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
    
    /// <summary>
    /// 地图配置文件
    /// </summary>
    private MapCfg map;
    
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
        map = resSvc.GetMapCfgData(Constants.MapID_MainCity);
        
        resSvc.AsyncLoadScene(map.sceneName,OpenMainCityWindow);
        audioSvc.PlayBGMusic(Constants.BGMainCity);
        
        // TODO 设置人物摄像机
    }
    
    private void OpenMainCityWindow()
    {
        // 加载主角
        LoadPlayer(map);
        // 加载相机
        LoadCamera(map);
        // 加载主城场景UI
        var mainCityWindow = gameRootResources.mainCityWindow;
        mainCityWindow.SetWindowState(true);
    }

    private void LoadPlayer(MapCfg map)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AsnCityPlayerPrefab,true);
        player.transform.position = map.playerBornPos;
        player.transform.localEulerAngles = map.playerBornRote;
        player.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
    } 
    
    private void LoadCamera(MapCfg map)
    {
        Camera.main.transform.position = map.mainCamPos;
        Camera.main.transform.localEulerAngles = map.mainCamRote;
    } 
    
}
