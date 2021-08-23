/****************************************************
    文件：LoginSys.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/12 21:53:2
    功能：登陆注册业务系统
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class LoginSys : SystemBase//GameRootMonoSingleton<LoginSys>
{
    public static LoginSys Instance;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        Debug.Log("Init LoginSys.");
    }

    /// <summary>
    /// 异步加载登陆场景和进度条，加载完成后再显示
    /// </summary>
    public void EnterLogin()
    {
        resSvc.AsyncLoadScene(Constants.SceneLogin,OpenLoginWindow);
        audioSvc.PlayBGMusic(Constants.BGLogin);
        gameRootResources.ShowTips("进入登陆界面成功1");
        gameRootResources.ShowTips("进入登陆界面成功2");
        gameRootResources.ShowTips("进入登陆界面成功3");
    }
    
    public void OpenLoginWindow()
    {
        var loginWindow = gameRootResources.loginWindow;
        loginWindow.SetWindowState(true);
    }
}
