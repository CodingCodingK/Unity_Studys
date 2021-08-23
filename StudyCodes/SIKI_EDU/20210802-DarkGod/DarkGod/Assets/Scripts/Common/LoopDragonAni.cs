/****************************************************
    文件：LoopDragonAni.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/20 7:52:18
    功能：控制登录界面的龙动画循环
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopDragonAni : MonoBehaviour
{
    private Animation ani;

    private void Start()
    {
        ani = transform.GetComponent<Animation>();
        if (ani)
        {
            InvokeRepeating("PlayDragonAni",0,20);
        }
    }

    private void PlayDragonAni()
    {
        if (ani)
        {
            ani.Play();
        }
    }
}
