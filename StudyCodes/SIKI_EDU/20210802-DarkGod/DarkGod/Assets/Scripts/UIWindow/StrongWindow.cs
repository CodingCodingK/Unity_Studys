/****************************************************
    文件：LoginWindow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/23
    功能：强化界面
*****************************************************/


using System;
using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class StrongWindow : WindowBase
{
    #region define
    // UI
    public Transform posBtnTrans;
    public Image curtImg;
    public Text txtStartLv;
    public Transform starTransGrp;
    public Text propHP1;
    public Text propHurt1;
    public Text propDef1;
    public Text propHP2;
    public Text propHurt2;
    public Text propDef2;
    public Image propArr1;
    public Image propArr2;
    public Image propArr3;

    public Text txtNeedLv;
    public Text txtCostCoin;
    public Text txtCostCrystal;
    
    public Text txtCoin;



    // others
    private Image[] imgs = new Image[6];
    private int curtIndex;
    private PlayerData pd;
    private StrongCfg nowData;
    private StrongCfg preData;

    #endregion
    

    protected override void InitWindow()
    {
        base.InitWindow();

        RegClickEvents();
        UpdateUI();
    }

    public void RefreshItems()
    {
        pd = GameRoot.Instance().PlayerData;
        
        SetText(txtCoin,pd.coin);
        switch (curtIndex)
        {
            case 0:
                SetSprite(curtImg,PathDefine.ItemToukui);
                break;
            case 1:
                SetSprite(curtImg,PathDefine.ItemBody);
                break;
            case 2:
                SetSprite(curtImg,PathDefine.ItemYaobu);
                break;
            case 3:
                SetSprite(curtImg,PathDefine.ItemHand);
                break;
            case 4:
                SetSprite(curtImg,PathDefine.ItemLeg);
                break;
            case 5:
                SetSprite(curtImg,PathDefine.ItemFoot);
                break;
        }
        
        SetText(txtStartLv,"- " + pd.strongArr[curtIndex] + "星级");
        int curtStarLv = pd.strongArr[curtIndex];
        for (int i = 0; i < starTransGrp.childCount; i++)
        {
            Image img = starTransGrp.GetChild(i).GetComponent<Image>();
            if (curtStarLv >= i + 1)
            {
                SetSprite(img,PathDefine.SpStar2);
            }
            else
            {
                SetSprite(img,PathDefine.SpStar1);
            }
        }
        // now data
        Debug.Log(curtStarLv);
        nowData = resSvc.GetStrongData(curtIndex, curtStarLv);
        SetText(propHP1,"+" + nowData.addhp);
        SetText(propHurt1,"+" + nowData.addhurt);
        SetText(propDef1,"+" + nowData.adddef);
        
        // pre data
        preData = resSvc.GetStrongData(curtIndex, curtStarLv+1);
        if (preData == null)
        {
            SetActive(propArr1,false);
            SetActive(propArr2,false);
            SetActive(propArr3,false);
            SetActive(propHP2,false);
            SetActive(propHurt2,false);
            SetActive(propDef2,false);
            SetText(txtNeedLv,nowData.minlv);
            SetText(txtCostCoin,"已到最高星级");
            SetText(txtCostCrystal,"已到最高星级");
        }
        else
        {
            
            SetActive(propArr1,true);
            SetActive(propArr2,true);
            SetActive(propArr3,true);
            SetActive(propHP2,true);
            SetActive(propHurt2,true);
            SetActive(propDef2,true);
            SetText(propHP2,"+" + preData.addhp);
            SetText(propHurt2,"+" + preData.addhurt);
            SetText(propDef2,"+" + preData.adddef);
            SetText(txtNeedLv,preData.minlv);
            SetText(txtCostCoin,nowData.coin);
            SetText(txtCostCrystal,nowData.crystal + "/" + pd.crystal);
        }
        
        SetText(txtCoin,pd.coin);
        
        
    }


    private void UpdateUI()
    {
        ClickPosItem(curtIndex);
    }

    #region Click Events

    private void RegClickEvents()
    {
        for (int i = 0; i < posBtnTrans.childCount; i++)
        {
            Image img = posBtnTrans.GetChild(i).GetComponent<Image>();
            
            OnClick(img.gameObject,(object obj)=>
            {
                audioSvc.PlayUIAudio(Constants.UIClickBtn);
                ClickPosItem((int)obj);
            },i);
            imgs[i] = img;
        }
        
    }

    private void ClickPosItem(int index)
    {
        PECommon.Log("Click Item "+ index);
        curtIndex = index;
        
        for (int i = 0; i < imgs.Length; i++)
        {
            Transform trans = imgs[i].transform;
            if (i == curtIndex)
            {
                SetSprite(imgs[i],PathDefine.ItemArrorBG);
                trans.localPosition = new Vector3(11f, trans.localPosition.y, 0f);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(222f,86f);
            }
            else
            {
                SetSprite(imgs[i],PathDefine.ItemPlatBG);
                trans.localPosition = new Vector3(-1f, trans.localPosition.y, 0f);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(188f, 60f);
            }
        }

        RefreshItems();
    }
    
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWindowState(false);
    }
    
    public void ClickStrongBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        // 客户端验证
        if (preData == null)
        {
            GameRootResources.Instance().ShowTips("已提升到最高星级");
        }
        else if (pd.level < preData.starlv || pd.coin < preData.coin || pd.crystal < preData.crystal)
        {
            GameRootResources.Instance().ShowTips("未满足升星要求");
        }
        else
        {
            // 服务端验证
            GameMsg msg = new GameMsg
            {
                cmd = (int) CMD.ReqStrong,
                reqStrong = new ReqStrong(){pos = curtIndex},
            };
            netSvc.SendMsg(msg);
        }
    }
    

    #endregion
   
    
}