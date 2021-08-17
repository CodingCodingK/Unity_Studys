/****************************************************
    文件：LoginWindow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/17 22:19:29
    功能：登陆注册界面
*****************************************************/

using System.Collections;
using System.Collections.Generic;
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
    
    
}
