/****************************************************
    文件：MainCitySys.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/14 21:49:17
    功能：副本业务系统
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PEProtocol;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonSys : SystemBase
{
    public static DungeonSys Instance;
    
    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        Debug.Log("Init DungeonSys.");
    }

    public void EnterDG()
    {
        OpenDGWindow();
    }

    public void OpenDGWindow()
    {
        gameRootResources.dungeonWindow.SetWindowState();
    }

    public void RspDungeon(GameMsg msg)
    {
        GameRoot.Instance().SetPlayerDataByDungeon(msg.rspDungeon);
        gameRootResources.dungeonWindow.SetWindowState(false);
        gameRootResources.mainCityWindow.SetWindowState(false);
        // TODO 加载战斗场景
        BattleSys.Instance.StartBattle(msg.rspDungeon.dgId);
        
    }
}
