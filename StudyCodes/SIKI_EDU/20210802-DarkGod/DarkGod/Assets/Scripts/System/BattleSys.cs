/****************************************************
    文件：BattleSys.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/10/7 21:49:17
    功能：战斗业务系统
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PEProtocol;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSys : SystemBase
{
    public static BattleSys Instance;
    public BattleMgr battleMgr;
    
    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        Debug.Log("Init BattleSys.");
    }

    public void StartBattle(int mapId)
    {
        GameObject go = new GameObject()
        {
            name = "BattleRoot",
        };
        go.transform.SetParent(GameRoot.Instance().transform);

        battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.Init(mapId);
        SetPlayerCtrlWindowState();
    }

    public void SetPlayerCtrlWindowState(bool isActive = true)
    {
        gameRootResources.playerCtrlWindow.SetWindowState(isActive);
    }

    public void SetMoveDir(Vector2 dir)
    {
        battleMgr.SetSelfPlayerMoveDir(dir);
    }
    
    public void ReqReleaseSkill(int index)
    {
        battleMgr.ReqReleaseSkill(index);
    }

}