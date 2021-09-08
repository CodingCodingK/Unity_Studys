/****************************************************
    文件：LoginWindow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/17 22:19:29
    功能：登陆注册界面
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class LoginWindow : WindowBase
{
    public InputField inputAccount;
    public InputField inputPassword;
    public Button btnEnter;
    public Button btnNotice;

    protected override void InitWindow()
    {
        base.InitWindow();
        // 获取本地存储的账号密码
        if (PlayerPrefs.HasKey("Account") && PlayerPrefs.HasKey("Password"))
        {
            inputAccount.text = PlayerPrefs.GetString("Account");
            inputPassword.text = PlayerPrefs.GetString("Password");
        }
        else
        {
            inputAccount.text = string.Empty;
            inputPassword.text = string.Empty;
        }
    }

    /// <summary>
    /// 点击Login画面登录按钮
    /// </summary>
    public void ClickEnterBtn()
    {
        audioSvc.PlayUIAudio(Constants.UILoginBtn);

        string account = inputAccount.text;
        string password = inputPassword.text;

        if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(password))
        {
            PlayerPrefs.SetString("Account",account);
            PlayerPrefs.SetString("Password",password);
            
            //TODO 发送网络请求，验证登录
            GameMsg msg = new GameMsg()
            {
                cmd = (int) CMD.ReqLogin,
                reqLogin = new ReqLogin
                {
                    acct = account,
                    pass = password
                }
            };
            netSvc.SendMsg(msg);
            
            //根据返回值，登录成功执行
            //LoginSys.Instance.RspLogin();
        }
        else
        {
            GameRootResources.Instance().ShowTips("账号或密码为空");
            
        }
    }
    
    /// <summary>
    /// 点击Login画面通知按钮
    /// </summary>
    public void ClickNoticeBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        GameRootResources.Instance().ShowTips("通知系统开发中");
    }
    
    
    
}
