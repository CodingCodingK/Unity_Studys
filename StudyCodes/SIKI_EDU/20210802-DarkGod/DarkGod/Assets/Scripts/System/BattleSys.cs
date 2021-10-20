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
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSys : SystemBase
{
    public static BattleSys Instance;
    public BattleMgr battleMgr;

    private int dgId;
    private double startTime;
    
    
    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        Debug.Log("Init BattleSys.");
    }

    public void StartBattle(int mapId)
    {
        dgId = mapId;
        GameObject go = new GameObject()
        {
            name = "BattleRoot",
        };
        go.transform.SetParent(GameRoot.Instance().transform);

        battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.Init(mapId, () =>
        {
            startTime = TimerSvc.Instance().GeyNowTime();
        });
        SetPlayerCtrlWindowState();
    }
    
    public void EndBattle(bool isWin,int restHp)
    {
        gameRootResources.playerCtrlWindow.SetWindowState(false);
        gameRootResources.dynamicWindow.RemoveAllHpItemInfo();

        if (isWin)
        {
            // TODO 发送结算战斗请求
            double endTime = TimerSvc.Instance().GeyNowTime();
            GameMsg msg = new GameMsg()
            {
                cmd = (int) CMD.ReqDungeonEnd,
                reqDungeonEnd = new ReqDungeonEnd()
                {
                    win = isWin,
                    dgId = dgId,
                    restHp = restHp,
                    costTime = (int)((endTime - startTime) / 1000),
                },
            };
        }
        else
        {
            SetBattleEndWindowState(FBEndType.Lose);
        }
    }

    public void DestroyBattle()
    {
        SetPlayerCtrlWindowState(false);
        SetBattleEndWindowState(FBEndType.None,false);
        gameRootResources.dynamicWindow.RemoveAllHpItemInfo();
        Destroy(battleMgr.gameObject);
    }

    public void SetPlayerCtrlWindowState(bool isActive = true)
    {
        gameRootResources.playerCtrlWindow.SetWindowState(isActive);
    }
    
    public void SetBattleEndWindowState(FBEndType type,bool isActive = true)
    {
        gameRootResources.battleEndWindow.SetWindowType(type);
        gameRootResources.battleEndWindow.SetWindowState(isActive);
    }

    public void SetMoveDir(Vector2 dir)
    {
        battleMgr.SetSelfPlayerMoveDir(dir);
    }
    
    public void ReqReleaseSkill(int index)
    {
        battleMgr.ReqReleaseSkill(index);
    }

    public Vector2 GetDirInput()
    {
        return gameRootResources.playerCtrlWindow.curtDir;
    }

}