/****************************************************
    文件：MapMgr.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/10/7 21:49:17
    功能：技能管理器
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PEProtocol;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapMgr: MonoBehaviour
{
    private int waveIndex = 1;
    private BattleMgr battleMgr;
    public TriggerData[] triggerArr;
    
    public void Init(BattleMgr battle)
    {
        battleMgr = battle;
        
        // 实例第一批怪物
        battleMgr.LoadMonsterByWaveID(waveIndex);

        Debug.Log("Init MapMgr.");
    }

    public void TriggerMonsterBorn(TriggerData trigger,int waveIndex)
    {
        BoxCollider co = trigger.gameObject.GetComponent<BoxCollider>();
        co.isTrigger = false;
        
        battleMgr.LoadMonsterByWaveID(waveIndex);
        battleMgr.ActiveCurrentBatchMonsters();

        battleMgr.triggerCheck = true;
    }

    public bool SetNextTriggerOn()
    {
        waveIndex++;
        foreach (var trigger in triggerArr)
        {
            if (trigger.triggerWave == waveIndex)
            {
                BoxCollider collider = trigger.GetComponent<BoxCollider>();
                collider.isTrigger = true;
                return true;
            }
        }

        return false;
    }

}