/****************************************************
    文件：MainCitySys.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/14 21:49:17
    功能：主城业务系统
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PEProtocol;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCitySys : SystemBase
{
    public static MainCitySys Instance;
    
    /// <summary>
    /// 地图配置文件
    /// </summary>
    private MapCfg map;
    
    /// <summary>
    /// 主角控制器
    /// </summary>
    private PlayerController playerCtrl;
    
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
        PECommon.Log("Enter MainCity...");
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
        //player.transform.position = map.playerBornPos;TODO why cant use this?
        player.transform.localPosition = map.playerBornPos;
        player.transform.localEulerAngles = map.playerBornRote;
        player.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
        Debug.Log(player.transform.parent);
        playerCtrl = player.GetComponent<PlayerController>();
        
        
    } 
    
    private void LoadCamera(MapCfg map)
    {
        Camera.main.transform.localPosition = map.mainCamPos;
        Camera.main.transform.localEulerAngles = map.mainCamRote;
        Debug.Log(Camera.main.transform.parent);
        
        playerCtrl.Init();
    } 
    
    public void SetMoveDir(Vector2 dir) {
        if (dir == Vector2.zero) {
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
        else {
            playerCtrl.SetBlend(Constants.BlendWalk);
        }
        playerCtrl.Dir = dir;
    }
    
}
