/****************************************************
    文件：LoginWindow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/23
    功能：通用购买界面
*****************************************************/


using System;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class BuyWindow : WindowBase
{
    #region define
    // UI
    public Text txtInfo;
    
    // others
    private int buyType;//0体力 1金币
    
    
    #endregion

    public void OpenBuyWindow(int type)
    {
        buyType = type;
        InitWindow();
        SetWindowState();
    }
    

    protected override void InitWindow()
    {
        base.InitWindow();
        RegClickEvents();
        RefreshUI();
    }

    private void RefreshUI()
    {
        switch (buyType)
        {
            case 0:
                // 体力
                txtInfo.text = "是否花费" + Constants.ColoredTxt("10钻石", TxtColor.Red) + "购买" + Constants.ColoredTxt("100体力", TxtColor.Green) + "？";
                break;
            case 1:
                // 金币
                txtInfo.text = "是否花费" + Constants.ColoredTxt("10钻石", TxtColor.Red) + "购买" + Constants.ColoredTxt("1000金币", TxtColor.Green) + "？";
                break;
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
    
    public void ClickSureBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        GameMsg msg = new GameMsg()
        {
            cmd = (int) CMD.ReqBuy,
            reqBuy = new ReqBuy()
            {
                type = buyType,
                cost = 10,
            }
        };
        netSvc.SendMsg(msg);
        // SetWindowState(false);
    }
    
    #endregion
   
    
}