/****************************************************
    文件：MainCitySys.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/14 21:49:17
    功能：主城业务系统
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PEProtocol;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MainCitySys : SystemBase
{
    public static MainCitySys Instance;
    
    /// <summary>
    /// 地图配置文件
    /// </summary>
    private MapCfg map;
    
    /// <summary>
    /// 主角控制器
    /// </summary>
    private PlayerController playerCtrl;
    
    /// <summary>
    /// 主角展示相机
    /// </summary>
    private Transform charCameraTrans;

    private AutoGuideCfg curtTaskData;
    private Transform[] npcPosTrans;
    private NavMeshAgent nav;
    private bool isNavGuiding;
    
    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        Debug.Log("Init MainCitySys.");
    }
    
    private void Update()
    {
        // nav
        if (isNavGuiding)
        {
            IsArrivedNavPos();
            playerCtrl.SetCam();
        }
        
    }
    
    /// <summary>
    /// 进入主城业务
    /// </summary>
    public void EnterMainCity()
    {
        map = resSvc.GetMapCfgData(Constants.MapID_MainCity);
        
        resSvc.AsyncLoadScene(map.sceneName,OpenMainCityWindow);
        audioSvc.PlayBGMusic(Constants.BGMainCity);
        
        // 设置人物摄像机
        if (charCameraTrans != null)
        {
            charCameraTrans.gameObject.SetActive(false);
        }
    }
    
    private void OpenMainCityWindow()
    {
        PECommon.Log("Enter MainCity...");
        // 加载主角
        LoadPlayer(map);
        // 加载相机
        LoadCamera(map);
        // 加载主城场景UI
        var mainCityWindow = gameRootResources.mainCityWindow;
        mainCityWindow.SetWindowState(true);

        GameRoot.Instance().gameObject.GetComponent<AudioListener>().enabled = false;
    }

    private void LoadPlayer(MapCfg map)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AsnCityPlayerPrefab,true);
        //player.transform.position = map.playerBornPos;TODO why cant use this?
        player.transform.localPosition = map.playerBornPos;
        player.transform.localEulerAngles = map.playerBornRote;
        player.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
        
        GameObject mainMapGO = GameObject.FindGameObjectWithTag("MapRoot");
        MainCityMap mainMap = mainMapGO.GetComponent<MainCityMap>();
        npcPosTrans = mainMap.NpcPosTrans;
        playerCtrl = player.GetComponent<PlayerController>();
        nav = player.GetComponent<NavMeshAgent>();
    } 
    
    private void LoadCamera(MapCfg map)
    {
        Camera.main.transform.localPosition = map.mainCamPos;
        Camera.main.transform.localEulerAngles = map.mainCamRote;
        
        playerCtrl.Init();
    } 
    
    /// <summary>
    /// 控制角色移动
    /// </summary>
    public void SetMoveDir(Vector2 dir)
    {
        StopNavTask();
        if (dir == Vector2.zero) {
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
        else {
            playerCtrl.SetBlend(Constants.BlendMove);
        }
        playerCtrl.Dir = dir;
    }

    private float startRotateY = 0f;
    public void SetStartRotateY()
    {
        startRotateY = playerCtrl.transform.localEulerAngles.y;
    }
    
    /// <summary>
    /// 控制角色旋转
    /// </summary>
    public void SetPlayerRotateY(float y)
    {
        playerCtrl.transform.localEulerAngles = new Vector3(0, startRotateY+y, 0);
    }
    

    public void OpenInfoWindow()
    {
        StopNavTask();
        if (charCameraTrans == null)
        {
            charCameraTrans = GameObject.FindGameObjectWithTag("charCamera").transform;
        }
        
        // 设置人物展示相机相对位置
        charCameraTrans.localPosition = playerCtrl.transform.position + playerCtrl.transform.forward * 2.8f + new Vector3(0,1.2f,0);
        charCameraTrans.localEulerAngles = new Vector3(0, 180 + playerCtrl.transform.localEulerAngles.y, 0);
        charCameraTrans.localScale = Vector3.one;
        charCameraTrans.gameObject.SetActive(true);
        
        gameRootResources.infoWindow.SetWindowState();
    }

    public void CloseInfoWindow()
    {
        if (charCameraTrans!=null)
        {
            charCameraTrans.gameObject.SetActive(false);
            gameRootResources.infoWindow.SetWindowState(false);
        }
    }

    #region Guide

    /// <summary>
    /// NavMeshAgent自动引导
    /// </summary>
    public void RunTask(AutoGuideCfg cfg)
    {
        nav.enabled = true;
        if (cfg != null)
        {
            curtTaskData = cfg;
        }
        
        // 解析任务数据
        if (curtTaskData.npcID != -1)
        {
            if (IsArrivedNavPos())
            {
                
            }
            else
            {
                isNavGuiding = true;
                nav.enabled = true;
                nav.isStopped = false;
                nav.speed = Constants.PlayerMoveSpeed;
                nav.SetDestination(npcPosTrans[curtTaskData.npcID].position);
                playerCtrl.SetBlend(Constants.BlendMove);
            }
        }
        else
        {
            // -1的情况，是没有实际NPC，直接出对话框
            OpenGuideWindow();
        }
    }

    public void StopNavTask()
    {
        if (isNavGuiding)
        {
            isNavGuiding = false;
            nav.isStopped = true;
            nav.enabled = false;
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
    }

    private bool IsArrivedNavPos()
    {
        float dis = Vector3.Distance(playerCtrl.transform.position,npcPosTrans[curtTaskData.npcID].position);
        // 判定距离小于0.5算找到
        if (dis < 0.5f )
        {
            StopNavTask();
            
            OpenGuideWindow();
            return true;
        }

        return false;
    }
    
    private void OpenGuideWindow()
    {
        // TODO
        gameRootResources.guideWindow.SetWindowState();
        
    }

    public AutoGuideCfg GetCurTaskData()
    {
        return curtTaskData;
    }
    
    /// <summary>
    /// 引导任务完成后执行，更新游戏内文本 
    /// </summary>
    public void RspGuide(GameMsg msg)
    {
        RspGuide rsp = msg.rspGuide;
        var txt = Constants.ColoredTxt("任务奖励金币: +" + curtTaskData.coin + ",经验: +" + curtTaskData.exp, TxtColor.Blue);
        gameRootResources.ShowTips(txt);
        switch (curtTaskData.actID)
        {
            // 与智者对话
            case 0:
                break;
            // 进入副本
            case 1:
                break;
            // 进入强化界面
            case 2:
                break;
            // 进入体力购买
            case 3:
                break;
            // 进入金币铸造
            case 4:
                break;
            // 进入世界聊天
            case 5:
                break;
        }

        GameRoot.Instance().SetPlayerDataByGuide(rsp);
        gameRootResources.mainCityWindow.RefreshUI();
    }
    
    #endregion

    #region Strong

    public void OpenStrongWindow()
    {
        gameRootResources.strongWindow.SetWindowState();
    }

    public void RspStrong(GameMsg msg)
    {
        RspStrong rsp = msg.rspStrong;
        var FightFrom = PECommon.GetFight(GameRoot.Instance().PlayerData);
        GameRoot.Instance().SetPlayerDataByStrong(rsp);
        var FightTo = PECommon.GetFight(GameRoot.Instance().PlayerData);
        var txt = Constants.ColoredTxt("升星成功，战力提升：" + (FightTo - FightFrom) , TxtColor.Blue);
        gameRootResources.ShowTips(txt);
        
        gameRootResources.strongWindow.RefreshItems();
    }
    #endregion
    
    #region Chat

    public void OpenChatWindow()
    {
        if (!gameRootResources.chatWindow.gameObject.activeSelf)
        {
            gameRootResources.chatWindow.SetWindowState();
        }
        else
        {
            gameRootResources.chatWindow.ClickCloseBtn();
        }
        
    }

    public void PushChat(GameMsg msg)
    {
        gameRootResources.chatWindow.AddChatMsg(msg.pushChat.name,msg.pushChat.chat);
    }
   
    #endregion

    #region Buy

    public void OpenBuyWindow(int type)
    {
        gameRootResources.buyWindow.OpenBuyWindow(type);
    }
    
    public void RspBuy(GameMsg msg)
    {
        RspBuy rsp = msg.rspBuy;
        string txt = string.Empty;
        switch (rsp.type)
        {
            case 0 :
                txt = Constants.ColoredTxt("购买成功，目前体力：" + rsp.power ,TxtColor.Blue);
                break;
            case 1 :
                txt = Constants.ColoredTxt("购买成功，目前金钱：" + rsp.coin ,TxtColor.Blue);
                break;
        }
        
        gameRootResources.ShowTips(txt);
        GameRoot.Instance().SetPlayerDataByBuy(rsp);
        gameRootResources.mainCityWindow.RefreshUI();
    }

    #endregion

    #region Power 体力自动增长

    public void PushPower(GameMsg msg)
    {
        GameRoot.Instance().SetPlayerDataByPower(msg.pushPower);
        if (gameRootResources.mainCityWindow.gameObject.activeSelf)
        {
            gameRootResources.mainCityWindow.RefreshUI();
        }
       
    }

    #endregion

    #region Task

    public void OpenTaskWindow()
    {
        gameRootResources.taskWindow.SetWindowState();
    }

    public void RspTask(GameMsg msg)
    {
        GameRoot.Instance().SetPlayerDataByTask(msg.rspTask);
        if (gameRootResources.mainCityWindow.gameObject.activeSelf)
        {
            gameRootResources.mainCityWindow.RefreshUI();
        }
        if (gameRootResources.taskWindow.gameObject.activeSelf)
        {
            gameRootResources.taskWindow.RefreshUI();
        }
    }
    
    public void PushTaskPrgs(GameMsg msg)
    {
        GameRoot.Instance().SetPlayerDataByTaskPrgs(msg.pushTaskPrgs);
        if (gameRootResources.taskWindow.gameObject.activeSelf)
        {
            gameRootResources.taskWindow.RefreshUI();
        }
    }
    #endregion

    #region 副本

    public void EnterDG()
    {
        StopNavTask();
        DungeonSys.Instance.EnterDG();
    }

    #endregion
}
