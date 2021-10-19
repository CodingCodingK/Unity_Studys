/****************************************************
    文件：TriggerData.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/10/19 22:34:38
    功能：地图关卡控制器
*****************************************************/

using System;
using UnityEngine;

public class TriggerData : MonoBehaviour
{
    public MapMgr mapMgr;
    public int triggerWave;
    
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            mapMgr.TriggerMonsterBorn(this,triggerWave);
        }
    }
    
    
}