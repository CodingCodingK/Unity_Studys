/****************************************************
    文件：MainCitySys.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/14 21:49:17
    功能：主城业务系统
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCitySys : SystemBase
{
    public static MainCitySys Instance;
    
    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        Debug.Log("Init MainCitySys.");
    }

    /// <summary>
    /// 进入主城业务
    /// </summary>
    public void EnterMainCity()
    {
        resSvc.AsyncLoadScene(Constants.SceneMainCity,OpenMainCityWindow);
        audioSvc.PlayBGMusic(Constants.BGMainCity);
        
        // TODO 设置人物摄像机
    }
    
    private void OpenMainCityWindow()
    {
        // TODO 加载主角
        
        // 加载主城场景UI
        var mainCityWindow = gameRootResources.mainCityWindow;
        mainCityWindow.SetWindowState(true);
    }
    
}
