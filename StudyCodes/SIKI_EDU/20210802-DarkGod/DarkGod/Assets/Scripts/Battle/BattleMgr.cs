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
         GameObject player = resSvc.LoadPrefab(PathDefine.AsnBattlePlayerPrefab);

         player.transform.position = mapData.playerBornPos;
         player.transform.localEulerAngles = mapData.playerBornRote;
         player.transform.localScale = Vector3.one;
    }
}
