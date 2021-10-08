/****************************************************
    文件：DungeonWindow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/10/7 22:38:5
    功能：副本选择界面
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class DungeonWindow : WindowBase
{
    #region define
    // UI
    public Transform[] DGBtnArr;
    public Transform curtPoint;

    // others
    private PlayerData pd;

    #endregion
    
    protected override void InitWindow()
    {
        base.InitWindow();

        pd = GameRoot.Instance().PlayerData;
        RegClickEvents();

        RefreshUI();
    }
    
    private void RefreshUI()
    {
        int id = pd.dg;
        for (int i = 0; i < DGBtnArr.Length; i++)
        {
            if (i < id % 10000)
            {
                SetActive(DGBtnArr[i].gameObject,true);
                if (i == id % 10000 - 1)
                {
                    curtPoint.SetParent(DGBtnArr[i].transform);
                    curtPoint.localPosition = new Vector3(0,102, 0);
                }
            }
            else
            {
                SetActive(DGBtnArr[i].gameObject,false);
            }
        }
    }
    
    #region Click Events

    private void RegClickEvents()
    {
        
    }
    
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWindowState(false);
    }
    
    public void ClickTaskBtn(int dgId)
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        var costPower = resSvc.GetMapCfgData(dgId).power;

        if (costPower > pd.power)
        {
            GameRootResources.Instance().ShowTips("体力不足！");
        }
        else
        {
            // TODO 副本
            netSvc.SendMsg(new GameMsg()
            {
                cmd = (int)CMD.ReqDungeon,
                reqDungeon = new ReqDungeon()
                {
                    dgId = dgId,
                }
            });
            
        }
    }

    #endregion
    
    
}
