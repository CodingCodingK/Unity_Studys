/****************************************************
    文件：GameRoot.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/12 21:49:2
    功能：游戏启动入口
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;

public class GameRoot : GameRootMonoSingleton<GameRoot>
{
    
    private void Start()
    {
        Debug.Log("Game Start.");

        ClearUIRoot();
        Init();
    }

    /// <summary>
    /// 初始化所有窗口，隐藏他们，除了显示Tips的DynamicWindow
    /// </summary>
    private void ClearUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }
        
        GameRootResources.Instance().dynamicWindow.SetWindowState(true);
    }

    /// <summary>
    /// GameRoot的流程
    /// </summary>
    private void Init()
    {
        // 设置不销毁
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(GameObject.Find("EventSystem"));
        
        //服务模块初始化
        NetSvc net = GetComponent<NetSvc>();
        net.InitSvc();
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();
        AudioSvc au = GetComponent<AudioSvc>();
        au.InitSvc();
        TimerSvc ti = GetComponent<TimerSvc>();
        ti.InitSvc();

        //业务系统初始化
        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();
        MainCitySys mainCity = GetComponent<MainCitySys>();
        mainCity.InitSys();
        
        //进入登陆场景并加载相应UI
        login.EnterLogin();
        
    }

    #region PlayerData
    
    /// <summary>
    /// 用户数据
    /// </summary>
    public PlayerData PlayerData
    {
        get;
        private set;
    }

    public void SetPlayerData(PlayerData pd)
    {
        PlayerData = pd;
    }

    public void SetPlayerName(string name)
    {
        PlayerData.name = name;
    }
    
    public void SetPlayerDataByGuide(RspGuide rsp)
    {
        PlayerData.level = rsp.lv;
        PlayerData.coin = rsp.coin;
        PlayerData.exp = rsp.exp;
        PlayerData.guideid = rsp.guideid;
    }
    
    public void SetPlayerDataByStrong(RspStrong rsp)
    {
        PlayerData.coin = rsp.coin;
        PlayerData.crystal = rsp.crystal;
        PlayerData.hp = rsp.hp;
        PlayerData.ad = rsp.ad;
        PlayerData.ap = rsp.ap;
        PlayerData.addef = rsp.addef;
        PlayerData.apdef = rsp.apdef;
        PlayerData.strongArr = rsp.strongArr;
       
    }

    public void SetPlayerDataByBuy(RspBuy rsp)
    {
        PlayerData.diamond = rsp.diamond;
        PlayerData.coin = rsp.coin;
        PlayerData.power = rsp.power;
    }
    
    public void SetPlayerDataByPower(PushPower push)
    {
        PlayerData.power = push.power;
    }

    #endregion
    
    
    
    
    
}
