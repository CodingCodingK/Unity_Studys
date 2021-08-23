/****************************************************
    文件：DynamicWindow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/23 19:45:34
    功能：动态UI元素（Tips）界面
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWindow : WindowBase
{
    public Animation tipsAni;
    public Text txtTips;
    private bool isTipsShow = false;
    private Queue<string> tipsPool = new Queue<string>();

    protected override void InitWindow()
    {
        base.InitWindow();
        SetActive(txtTips,false);
    }
    
    private void Update()
    {
        if (tipsPool?.Count > 0 && !isTipsShow)
        {
            lock (tipsPool)
            {
                SetTips(tipsPool.Dequeue());
                isTipsShow = true;
            }
            
        }
    }

    public void AddTips(string tip)
    {
        lock (tipsPool)
        {
            tipsPool.Enqueue(tip);
        }
    }
    
    private void SetTips(string tips)
    {
        SetActive(txtTips,true);
        SetText(txtTips,tips);

        AnimationClip clip = tipsAni.GetClip("TipsShowAni");
        tipsAni.Play();
        
        // 延时关闭激活状态
        StartCoroutine(AniPlayDone(clip.length, () =>
        {
            SetActive(txtTips,false);
            isTipsShow = false;
        }));
    }

    private IEnumerator AniPlayDone(float sec, Action action)
    {
        yield return new WaitForSeconds(sec);
        if (action!=null)
        {
            action();
        }
    }
}
