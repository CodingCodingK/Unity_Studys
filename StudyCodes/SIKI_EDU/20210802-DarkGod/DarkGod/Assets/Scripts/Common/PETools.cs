/****************************************************
    文件：PETools.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/24 22:30:59
    功能：工具类Utility
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PETools
{
    /// <summary>
    /// 返回一个从min到max的随机整数，也可以带上random种子
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="rd"></param>
    /// <returns></returns>
    public static int RDInt(int min,int max,System.Random rd = null)
    {
        if (rd == null)
        {
            rd = new System.Random();
        }

        return rd.Next(min, max + 1);
    }
    
}
