/****************************************************
    文件：WindowBase.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/17 23:41:36
    功能：UI界面基类
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowBase : MonoBehaviour
{
    public ResSvc resSvc = null;

    public void SetWindowState(bool isActive = true)
    {
        if (gameObject.activeSelf != isActive)
        {
            gameObject.SetActive(isActive);
        }

        if (isActive)
        {
            InitWindow();
        }
        else
        {
            ClearWindow();
        }
    }

    protected virtual void InitWindow()
    {
        resSvc = ResSvc.Instance();
    }
    
    protected virtual void ClearWindow()
    {
        resSvc = null;
    }

    #region Tool Functions

    protected void SetActive(GameObject go, bool isActive = true)
    {
        go.SetActive(isActive);
    }

    protected void SetActive(Transform tf, bool isActive = true)
    {
        tf.gameObject.SetActive(isActive);
    }
    
    protected void SetActive(RectTransform rtf, bool isActive = true)
    {
        rtf.gameObject.SetActive(isActive);
    }
    
    protected void SetActive(Image img, bool isActive = true)
    {
        img.gameObject.SetActive(isActive);
    }
    
    protected void SetActive(Text txt, bool isActive = true)
    {
        txt.gameObject.SetActive(isActive);
    }


    protected void SetText(Text txt, string context = "")
    {
        txt.text = context;
    }
    
    protected void SetText(Text txt, int num = 0)
    {
        SetText(txt, num.ToString());
    }
    
    protected void SetText(Transform tf, string context = "")
    {
        SetText(tf.GetComponent<Text>(),context);
    }
    
    protected void SetText(Transform tf, int num = 0)
    {
        SetText(tf.GetComponent<Text>(),num);
    }
    #endregion
    
}