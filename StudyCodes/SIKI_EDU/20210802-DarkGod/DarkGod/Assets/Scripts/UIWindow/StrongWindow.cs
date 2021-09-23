/****************************************************
    文件：LoginWindow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/23
    功能：强化界面
*****************************************************/


using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class StrongWindow : WindowBase
{
    #region define
    // UI
    public Transform posBtnTrans;
    
    // others
    private Image[] imgs = new Image[6];
    private int curtIndex;

    #endregion
    

    protected override void InitWindow()
    {
        base.InitWindow();

        RegClickEvents();
        ClickPosItem(0);
    }

    private void RegClickEvents()
    {
        for (int i = 0; i < posBtnTrans.childCount; i++)
        {
            Image img = posBtnTrans.GetChild(i).GetComponent<Image>();
            
            OnClick(img.gameObject,(obj)=>
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
                trans.localPosition = new Vector3(150, trans.position.y, trans.position.z);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(250f/220f *188.1739f, 95f/85f * 59.11288f);
            }
            else
            {
                SetSprite(imgs[i],PathDefine.ItemPlatBG);
                trans.localPosition = new Vector3(140, trans.position.y, trans.position.z);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(188.1739f, 59.11288f);
            }
            
        }
    }
    
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWindowState(false);
    }
    
}