/****************************************************
    文件：BattleEndWindow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/10/20
    功能：战斗结算界面
*****************************************************/


using System;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class BattleEndWindow: WindowBase
{
    #region define
    // UI
    public Transform rewardTrans;
    public Button btnClose;
    public Button btnExit;
    public Button btnSure;
    public Text txtTime;
    public Text txtRestHp;
    public Text txtReward;
    
    
    // others
    private FBEndType endType = FBEndType.None;


    #endregion
    
    protected override void InitWindow()
    {
        base.InitWindow();
        RefreshUI();
        
    }

    private void RefreshUI()
    {
        if (endType.Equals(FBEndType.Pause))
        {
            SetActive(rewardTrans,false);
            SetActive(btnExit.gameObject,true);
            SetActive(btnClose.gameObject,true);
            
        }
        else if (endType.Equals(FBEndType.Win))
        {
            
        }
        else if (endType.Equals(FBEndType.Lose))
        {
            SetActive(rewardTrans,false);
            SetActive(btnExit.gameObject,true);
            SetActive(btnClose.gameObject,false);
        }
    }

    public void SetWindowType(FBEndType type)
    {
        endType = type;
    }
    
    #region Click Events

    private void RegClickEvents()
    {
        
    }
    
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        BattleSys.Instance.battleMgr.isPauseGame = false;
        SetWindowState(false);
    }
    
    public void ClickExitBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        // 进入主城，销毁战斗
        MainCitySys.Instance.EnterMainCity();
        BattleSys.Instance.DestroyBattle();
    }
    
    public void ClickSureBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        // TODO 进入主城，销毁战斗，打开副本界面
    }
    
    #endregion
}

public enum FBEndType
{
    None,
    Pause,
    Win,
    Lose,
}
