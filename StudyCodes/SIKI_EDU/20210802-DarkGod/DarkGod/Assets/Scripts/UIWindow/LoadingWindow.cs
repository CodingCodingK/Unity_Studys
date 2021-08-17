/****************************************************
    文件：LoadingWindow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/12 22:43:17
    功能：Unknown
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingWindow : WindowBase
{
    public Text txtTips;
    public Image imgFG;
    public Image imgPoint;
    public Text txtPrg;

    private float fgWidth;

    protected override void InitWindow()
    {
        base.InitWindow();
        
        fgWidth = imgFG.GetComponent<RectTransform>().sizeDelta.x;
        SetText(txtTips,"这是一条tip！");
        SetText(txtPrg, "0%");
        imgPoint.transform.localPosition = new Vector3(-fgWidth/2, 0, 0);
        imgFG.fillAmount = 0;
    }

    public void SetProgress(float prg)
    {
        SetText(txtPrg, (int) (prg) + "%");
        imgFG.fillAmount = prg / 100f;
        imgPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(-fgWidth/2 + imgFG.fillAmount * fgWidth,  0);
        //imgPoint.transform.localPosition = new Vector3(-fgWidth, 0, 0);
    }
}
