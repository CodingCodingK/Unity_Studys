/****************************************************
    文件：InfoWindow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/17 22:32:53
    功能：角色面板
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class InfoWindow : WindowBase
{
    #region Info UI Define
    
    public Text txtInfo;
    public Text txtExp;
    public Image imgExpPrg;
    public Text txtPower;
    public Image imgPowerPrg;
    public Text txtJob;
    public Text txtFight;
    public Text txtHp;
    public Text txtAtk;
    public Text txtDef;
    public RawImage imgChar;
    private Vector2 startPos;
    
    #endregion
    
    #region DetailInfo UI Define

    public Transform transDetailInfo;
    public Text txtDetailHp;
    public Text txtDetailAd;
    public Text txtDetailAp;
    public Text txtDetailAddef;
    public Text txtDetailApdef;
    public Text txtDetailDodge;
    public Text txtDetailPierce;
    public Text txtDetailCritical;

    #endregion
    
    
    protected override void InitWindow()
    {
        base.InitWindow();
        SetActive(transDetailInfo,false);
        RegTouchEvents();
        RefreshUI();
    }

    private void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance().PlayerData;
        var levelExp = PECommon.GetExpMaxValByLv(pd.level);
        var levelPower = PECommon.GetPowerLimit(pd.level);
        var levelFight = PECommon.GetFight(pd);
        SetText(txtInfo,$"{pd.name} LV.{pd.level}");
        SetText(txtExp,$"{pd.exp}/{levelExp}");
        imgExpPrg.fillAmount = 1.0f * pd.exp / levelExp;
        SetText(txtPower,$"{pd.power}/{levelPower}");
        imgPowerPrg.fillAmount = 1.0f * pd.power / levelPower;
        SetText(txtJob,$"暗夜刺客");
        SetText(txtFight,$"{levelFight}");
        SetText(txtHp,$"{pd.hp}");
        SetText(txtAtk,$"{pd.ad + pd.ap}");
        SetText(txtDef,$"{pd.addef + pd.apdef}");
        
        // Detail Info
        SetText(txtDetailHp,pd.hp);
        SetText(txtDetailAd,pd.ad);
        SetText(txtDetailAp,pd.ap);
        SetText(txtDetailAddef,pd.addef);
        SetText(txtDetailApdef,pd.apdef);
        SetText(txtDetailDodge,pd.dodge+"%");
        SetText(txtDetailPierce,pd.pierce+"%");
        SetText(txtDetailCritical,pd.critical+"%");

    }

    #region Click Events

    private void RegTouchEvents()
    {
        OnClickDown(imgChar.gameObject, data =>
        {
            startPos = data.position;
            MainCitySys.Instance.SetStartRotateY();
        });
        
        OnDrag(imgChar.gameObject, data =>
        {
            float rotate = (startPos.x - data.position.x) * 0.5f;// 0.5是为了减缓 拖拽-旋转 速度
            MainCitySys.Instance.SetPlayerRotateY(rotate);
        });
    }
    
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.Instance.CloseInfoWindow();
    }
    
    public void ClickDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetActive(transDetailInfo,true);
    }
    
    public void ClickCloseDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetActive(transDetailInfo,false);
    }
    
    #endregion
    
}
