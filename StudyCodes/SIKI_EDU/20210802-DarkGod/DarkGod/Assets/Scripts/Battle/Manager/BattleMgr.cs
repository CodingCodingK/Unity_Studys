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
using PEProtocol;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleMgr: SystemBase
{
    public static BattleMgr Instance;

    private StateMgr stateMgr;
    private SkillMgr skillMgr;
    private MapMgr mapMgr;
    private EntityPlayer entityPlayer;
    private MapCfg mapCfg;

    private Dictionary<string, EntityMonster> monsterDic = new Dictionary<string, EntityMonster>();
    
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
         entityPlayer.SetBattleProps(props);
         
         entityPlayer.Idle();
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
                
                monster.SetActive(false);
                monsterDic.Add(monster.name, em);
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

    private void ReleaseNormalAtk()
    {
        
    }
    
    private void ReleaseSkill1()
    {
        entityPlayer.Attack(101);
    }

    private void ReleaseSkill2()
    {
        
    }
    
    private void ReleaseSkill3()
    {
        
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

    public void ActiveCurrentBatchMonsters()
    {
        TimerSvc.Instance().AddTimeTask(i =>
        {
            foreach (var monster in monsterDic)
            {
                monster.Value.controller.gameObject.SetActive(true);
            }
        },500);
    }

    #endregion

}
