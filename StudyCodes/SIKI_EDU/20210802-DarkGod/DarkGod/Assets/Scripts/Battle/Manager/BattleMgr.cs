/****************************************************
    文件：BattleMgr.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/10/7 21:49:17
    功能：战斗业务管理器
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using PEProtocol;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleMgr: SystemBase
{
    public static BattleMgr Instance;

    private StateMgr stateMgr;
    private SkillMgr skillMgr;
    private MapMgr mapMgr;
    public EntityPlayer entityPlayer;
    private MapCfg mapCfg;

    private Dictionary<string, EntityMonster> monsterDic = new Dictionary<string, EntityMonster>();
    public bool triggerCheck;
    
    public void Init(int mapId)
    {
        base.InitSys();
        Instance = this;

        stateMgr = gameObject.AddComponent<StateMgr>();
        stateMgr.Init();
        skillMgr = gameObject.AddComponent<SkillMgr>();
        skillMgr.Init();
        
        // 加载地图及场景
        mapCfg = resSvc.GetMapCfgData(mapId);
        resSvc.AsyncLoadScene(mapCfg.sceneName, () =>
        {
            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            mapMgr = map.GetComponent<MapMgr>();
            mapMgr.Init(Instance);
            
            // 初始化地图预制体信息
            map.transform.localPosition = Vector3.zero;
            map.transform.localScale = Vector3.one;
            Camera.main.transform.position = mapCfg.mainCamPos;
            Camera.main.transform.localEulerAngles = mapCfg.mainCamRote;

            LoadPlayer(mapCfg);
            
            audioSvc.PlayBGMusic(Constants.BGHuangYe);
        });

        Debug.Log("Init BattleMgr.");
    }

    private void Update()
    {
        foreach (var item in monsterDic)
        {
            EntityMonster em = item.Value;
            em.TickAILogic();
        }
        
        // 检测当前批次怪物是否全部死亡
        if (triggerCheck && monsterDic.Count == 0)
        {
            var isAllClear = mapMgr.SetNextTriggerOn();
            triggerCheck = false;
            
            if (isAllClear)
            {
                // TODO 战斗结算
                
            }
        }
    }

    private void LoadPlayer(MapCfg mapData)
    {
         GameObject player = resSvc.LoadPrefab(PathDefine.AsnBattlePlayerPrefab,true);
         // 初始位置
         player.transform.localPosition = mapData.playerBornPos;
         player.transform.localEulerAngles = mapData.playerBornRote;
         player.transform.localScale = Vector3.one;
         // 初始化
         PlayerData pd = GameRoot.Instance().PlayerData;
         BattleProps props = new BattleProps()
         {
             hp = pd.hp,
             ad = pd.ad,
             ap = pd.ap,
             addef = pd.addef,
             apdef = pd.apdef,
             dodge = pd.dodge,
             pierce = pd.pierce,
             critical = pd.critical,
         };
         PlayerController playerCtrl = player.GetComponent<PlayerController>();
         playerCtrl.Init();
         // 给其持有
         entityPlayer = new EntityPlayer()
         {
             stateMgr = stateMgr,
             controller = playerCtrl,
             skillMgr = skillMgr,
             battleMgr = Instance,
         };
         entityPlayer.Name = "AssassinBattle";
         entityPlayer.SetBattleProps(props);
         
         entityPlayer.Idle();

         // 动画组件有问题,特殊处理
         TimerSvc.Instance().AddTimeTask(i =>
         {
             entityPlayer.controller.ani.applyRootMotion = true;
         }, 1000);
         
         // 激活第一批怪物
         ActiveCurrentBatchMonsters();
        
    }

    public void LoadMonsterByWaveID(int wave)
    {
        for (var i = 0; i < mapCfg.monsterList.Count; i++)
        {
            var md = mapCfg.monsterList[i];
            if (md.mWave == wave)
            {
                var monster = resSvc.LoadPrefab(md.mCfg.resPath, true);
                // 初始位置
                monster.transform.localPosition = md.mBornPos;
                monster.transform.localEulerAngles = md.mBornRote;
                monster.transform.localScale = Vector3.one;
                // 初始化
                monster.name = "m" + md.mWave + "_" + md.mIndex;
                MonsterController mc = monster.GetComponent<MonsterController>();
                mc.Init();
                // 给其持有
                EntityMonster em = new EntityMonster()
                {
                    battleMgr = Instance,
                    controller = mc,
                    stateMgr = stateMgr,
                    skillMgr = skillMgr,
                };
                em.md = md;
                em.SetBattleProps(md.mCfg.props);
                em.Name = monster.name;
                
                monster.SetActive(false);
                monsterDic.Add(monster.name, em);
                
                if (md.mCfg.mType == MonsterType.Normal)
                {
                    gameRootResources.dynamicWindow.AddHpItemInfo(monster.name,mc.hpRoot,em.md.mCfg.props.hp);
                }
                else if (md.mCfg.mType == MonsterType.Boss)
                {
                    // Boss血条显示
                    gameRootResources.playerCtrlWindow.SetBossHpBarState(true);
                }
            }
        }
    }

    #region Control

    public void SetSelfPlayerMoveDir(Vector2 dir)
    {
        if (!entityPlayer.canControl)
        {
            return;
        }

        if (entityPlayer.curtAniState == AniState.Idle || entityPlayer.curtAniState == AniState.Move)
        {
            // 设置玩家移动
            if (dir == Vector2.zero)
            {
                entityPlayer.Idle();
                // 可以放到Idle状态的Enter中
                entityPlayer.SetDir(Vector2.zero);
            }
            else
            {     
                // 不可以放到Move状态的Enter中,因为不是来回进入此状态,而方向会随时变化
                entityPlayer.SetDir(dir);
                entityPlayer.Move();
            } 
        }

    }

    #endregion

    #region Skill

    public void ReqReleaseSkill(int id)
    {
        switch (id)
        {
            case 0:
                ReleaseNormalAtk();
                break;
            case 1:
                ReleaseSkill1();
                break;
            case 2:
                ReleaseSkill2();
                break;
            case 3:
                ReleaseSkill3();
                break;
                
        }
    }

    private int[] comboArr = new[] {111, 112, 113, 114, 115};
    public int comboIndex = 0;
    public double lastAtkTime = -1;
    private void ReleaseNormalAtk()
    {
        if (entityPlayer.curtAniState == AniState.Attack)
        {
            double nowAtkTime = TimerSvc.Instance().GeyNowTime();
            // 在500ms内进行第二次攻击，存数据
            if (nowAtkTime - lastAtkTime < Constants.ComboSpace && lastAtkTime != -1)
            {
                if (comboIndex < comboArr.Length - 1)
                {
                    comboIndex++;
                    entityPlayer.comboQue.Enqueue(comboArr[comboIndex]);
                    lastAtkTime = nowAtkTime; 
                }
                else
                {
                    // 此处是上次攻击状态已打出5下，但是还未回归到Idle状态的一小段时间内。
                    comboIndex = 0;
                    lastAtkTime = 0;
                }

            }
        }
        else if (entityPlayer.curtAniState == AniState.Idle || entityPlayer.curtAniState == AniState.Move)
        {
            // 重置普攻
            lastAtkTime = TimerSvc.Instance().GeyNowTime();
            comboIndex = 0;
            entityPlayer.Attack(comboArr[comboIndex]);
        }
        
        
    }
    
    private void ReleaseSkill1()
    {
        entityPlayer.Attack(101);
    }

    private void ReleaseSkill2()
    {
        entityPlayer.Attack(102);
    }
    
    private void ReleaseSkill3()
    {
        entityPlayer.Attack(103);
    }

    public Vector2 GetDirInput()
    {
        return BattleSys.Instance.GetDirInput();
    }
    #endregion

    #region Entity

    public List<EntityMonster> GetEntityMonsters()
    {
        List<EntityMonster> monsterList = new List<EntityMonster>();
        foreach (var item in monsterDic)
        {
            monsterList.Add(item.Value);
        }

        return monsterList;
    }

    public void RemoveMonster(string key)
    {
        if (monsterDic.TryGetValue(key,out var entityMonster))
        {
            monsterDic.Remove(key);
            gameRootResources.dynamicWindow.RemoveHpItemInfo(key);
        }
    }

    public void ActiveCurrentBatchMonsters()
    {
        TimerSvc.Instance().AddTimeTask(i =>
        {
            foreach (var monster in monsterDic)
            {
                monster.Value.controller.gameObject.SetActive(true);
                monster.Value.Born();
                // 出生后1秒进入Idle状态
                TimerSvc.Instance().AddTimeTask(o =>
                {
                    monster.Value.Idle();
                }, 1000);
            }
        },500);
    }

    public bool CanRlsSkill()
    {
        return entityPlayer.canSkill;
    }

    #endregion

}
