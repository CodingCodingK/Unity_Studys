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
    public Animation ani;
    public Transform[] itemsTrans;

    private int dgId;
    private int costTime;
    private int restHp;

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
            SetActive(rewardTrans,false);
            SetActive(btnExit.gameObject,false);
            SetActive(btnClose.gameObject,false);

            MapCfg cfg = resSvc.GetMapCfgData(dgId);
            int min = costTime / 60;
            int sec = costTime % 60;
            int coin = cfg.coin;
            int exp = cfg.exp;
            int crystal = cfg.crystal;
            
           
            SetText(txtTime,min + ":" + sec);
            SetText(txtRestHp,restHp);
            SetText(txtReward,Constants.ColoredTxt(coin + "金币 ",TxtColor.Yellow) 
                              + Constants.ColoredTxt(exp + "经验 ",TxtColor.Green) 
                              + Constants.ColoredTxt(crystal + "水晶",TxtColor.Blue));
            
            timerSvc.AddTimeTask(i =>
            {
                SetActive(rewardTrans);
                foreach (var item in itemsTrans)
                {
                    item.gameObject.SetActive(true);
                }
                SetActive(btnSure.gameObject,true);
                ani.Play();
                // 省略UI动画对应的音乐
            },1000);
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

    public void SetBattleEndData(int dgId,int costTime,int restHp)
    {
        this.dgId = dgId;
        this.costTime = costTime;
        this.restHp = restHp;
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
        // 进入主城，销毁战斗，打开副本界面
        MainCitySys.Instance.EnterMainCity();
        BattleSys.Instance.DestroyBattle();
        DungeonSys.Instance.EnterDG();
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
