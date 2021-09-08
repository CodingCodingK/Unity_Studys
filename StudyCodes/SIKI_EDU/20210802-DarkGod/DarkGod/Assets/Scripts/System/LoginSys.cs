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
using PEProtocol;
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
        gameRootResources.ShowTips("加载音乐资源...成功");
        gameRootResources.ShowTips("加载动画资源...成功");
    }
    
    public void OpenLoginWindow()
    {
        var loginWindow = gameRootResources.loginWindow;
        loginWindow.SetWindowState(true);
    }

    /// <summary>
    /// 登录成功后执行，进入创建人物界面
    /// </summary>
    public void RspLogin(GameMsg msg)
    {
        gameRootResources.ShowTips("登陆成功");
        GameRoot.Instance().SetPlayerData(msg.rspLogin.playerData);

        if (string.IsNullOrEmpty(msg.rspLogin.playerData.name))
        {
            // 打开角色创建页面
            gameRootResources.createWindow.SetWindowState(true);}
        else
        {
            // TODO 进入主城
        }

        // 关闭登录页面
        gameRootResources.loginWindow.SetWindowState(false);
    }
}
