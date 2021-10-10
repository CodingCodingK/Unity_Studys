/****************************************************
    文件：SkillMgr.cs
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

public class SkillMgr: MonoBehaviour
{
    private ResSvc resSvc;
    private TimerSvc timerSvc;
    public void Init()
    {
        resSvc = ResSvc.Instance();
        timerSvc = TimerSvc.Instance();
        Debug.Log("Init SkillMgr.");
    }

    /// <summary>
    /// 技能效果表现
    /// </summary>
    public void AttackEffect(EntityBase entity,int skillID)
    {
        SkillCfg skillData = resSvc.GetSkillCfgData(skillID);
        entity.SetAction(skillData.aniAction);
        entity.SetFX(skillData.fx,skillData.skillTime);
        
        timerSvc.AddTimeTask(i =>
        {
            entity.Idle();
            
        }, skillData.skillTime);
    }

}