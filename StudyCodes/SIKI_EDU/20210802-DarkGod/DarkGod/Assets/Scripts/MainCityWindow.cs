/****************************************************
    文件：MainCityWIndow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/14 21:38:54
    功能：主城窗口
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class MainCityWindow : WindowBase
{
    public Text txtFight;
    public Text txtPower;
    public Image imgPowerPrg;
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;
    
    protected override void InitWindow()
    {
        base.InitWindow();

        RefreshUI();
    }

    /// <summary>
    /// 根据数据库取得的 PlayerData 更新 UI
    /// </summary>
    private void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance().PlayerData;
        SetText(txtFight,PECommon.GetFight(pd));
        SetText(txtPower,$"{pd.power}/{PECommon.GetPowerLimit(pd.level)}");
        imgPowerPrg.fillAmount = pd.power * 1.0f / PECommon.GetPowerLimit(pd.level);
        SetText(txtLevel,pd.level);
        SetText(txtName,pd.name);
        
    }
    
    
}
