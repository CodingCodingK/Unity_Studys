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
using UnityEngine;

public class GameRoot : GameRootMonoSingleton<GameRoot>
{
    private void Start()
    {
        Debug.Log("Game Start.");
        Init();
    }

    private void Init()
    {
        //服务模块初始化
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();

        //业务系统初始化
        LoginSys login = GetComponent<LoginSys>();
        login.InitLogin();
        
        //进入登陆场景并加载相应UI
        login.EnterLogin();

    }
}
