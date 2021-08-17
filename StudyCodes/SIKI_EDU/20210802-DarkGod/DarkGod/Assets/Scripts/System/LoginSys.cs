/****************************************************
    文件：LoginSys.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/12 21:53:2
    功能：登陆注册业务系统
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSys : GameRootMonoSingleton<LoginSys>
{
    public void InitLogin()
    {
        Debug.Log("Init LoginSys.");
    }

    /// <summary>
    /// 异步加载登陆场景和进度条，加载完成后再显示
    /// </summary>
    public void EnterLogin()
    {
        ResSvc.Instance().AsyncLoadScene(Constants.SceneLogin,OpenLoginWindow);
    }
    
    public void OpenLoginWindow()
    {
        var loginWindow = GameRootResources.Instance().loginWindow;
        loginWindow.SetWindowState(true);
    }
}
