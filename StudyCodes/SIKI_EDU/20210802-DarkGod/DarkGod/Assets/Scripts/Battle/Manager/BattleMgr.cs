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
    
    public void Init(int mapId)
    {
        base.InitSys();
        Instance = this;

        stateMgr = gameObject.AddComponent<StateMgr>();
        stateMgr.Init();
        skillMgr = gameObject.AddComponent<SkillMgr>();
        skillMgr.Init();
        
        // 加载地图及场景
        MapCfg mapData = resSvc.GetMapCfgData(mapId);
        resSvc.AsyncLoadScene(mapData.sceneName, () =>
        {
            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            mapMgr = map.GetComponent<MapMgr>();
            mapMgr.Init();
            
            // 初始化地图预制体信息
            map.transform.localPosition = Vector3.zero;
            map.transform.localScale = Vector3.one;
            Camera.main.transform.position = mapData.mainCamPos;
            Camera.main.transform.localEulerAngles = mapData.mainCamRote;

            LoadPlayer(mapData);
            
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
         
         entityPlayer.Idle();
         
        
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
   
}
