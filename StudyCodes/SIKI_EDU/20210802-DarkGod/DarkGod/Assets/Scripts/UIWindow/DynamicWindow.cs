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
    public Transform hpItemRoot;
    
    private bool isTipsShow = false;
    private Queue<string> tipsPool = new Queue<string>();
    private Dictionary<string, ItemEntityHp> itemDic = new Dictionary<string, ItemEntityHp>();

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

    #region Tips相关

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
    
    #endregion

    #region ItemEntityHP相关

    public void AddHpItemInfo(string mName,Transform trans,int hp)
    {
        if(!itemDic.TryGetValue(mName,out ItemEntityHp item))
        {
            GameObject go = resSvc.LoadPrefab(PathDefine.ItemEntityHpPrefab,true);
            go.transform.SetParent(hpItemRoot);
            go.transform.localPosition = new Vector3(-1000, 0, 0);
            ItemEntityHp ieh = go.GetComponent<ItemEntityHp>();
            ieh.SetItemInfo(trans,hp);
            itemDic.Add(mName,ieh);
        }
    }
    
    public void RemoveHpItemInfo(string mName)
    {
        if(itemDic.TryGetValue(mName,out ItemEntityHp item))
        {
            Destroy(item.gameObject);
            itemDic.Remove(mName);
        }
    }

    public void SetDodge(string key)
    {
        if (itemDic.TryGetValue(key, out var item))
        {
            item.SetDodge();
        }
    }
    
    public void SetCritical(string key,int critical)
    {
        if (itemDic.TryGetValue(key, out var item))
        {
            item.SetCritical(critical);
        }
    }
    
    public void SetHurt(string key,int hurt)
    {
        if (itemDic.TryGetValue(key, out var item))
        {
            item.SetHurt(hurt);
        }
    }
    
    public void SetHpVal(string key,int oldVal,int newVal)
    {
        if (!string.IsNullOrEmpty(key))
        {
            if (itemDic.TryGetValue(key, out var item))
            {
                item.SetHpVal(oldVal,newVal);
            } 
        }
    }

    #endregion
}
