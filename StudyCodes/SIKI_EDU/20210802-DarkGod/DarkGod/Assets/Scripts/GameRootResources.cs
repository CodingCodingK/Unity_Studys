/****************************************************
    文件：GameRootResources.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/12 23:23:31
    功能：自制：存放GameRoot的所有资源，比如窗口等
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// sth like WindowManager etc.
/// </summary>
public class GameRootResources : GameRootMonoSingleton<GameRootResources>
{
    public LoadingWindow loadingWindow;

    public LoginWindow loginWindow;

    public DynamicWindow dynamicWindow;
    
    public void ShowTips(string tip)
    {
       dynamicWindow.AddTips(tip);
    }
}
