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
        // 设置技能 动作编号
        entity.SetAction(skillData.aniAction);
        // 设置技能特效 特效名、特效开启持续时间
        entity.SetFX(skillData.fx,skillData.skillTime);

        // 设置技能位移 适配多段位移、技能位移延迟
        CalcSkillMove(entity, skillData);

        entity.canControl = false;
        entity.SetDir(Vector2.zero);
        
        // 技能时间结束后 设置状态归零 
        timerSvc.AddTimeTask(i =>
        {
            entity.Idle();
            
        }, skillData.skillTime);
    }

    /// <summary>
    /// 设置技能位移 适配多段位移、技能位移延迟
    /// </summary>
    private void CalcSkillMove(EntityBase entity,SkillCfg skillData)
    {
        List<int> skillMoveList = skillData.skillMoveLst;

        int sum = 0;
        foreach (var skillMove in skillMoveList)
        {
            SkillMoveCfg skillMoveData = resSvc.GetSkillMoveCfgData(skillMove);
            var speed = skillMoveData.moveDis / (skillMoveData.moveTime / 1000f);
            sum += skillMoveData.delayTime;
            if (sum > 0)
            {
                timerSvc.AddTimeTask(i =>
                {
                    entity.SetSkillMoveState(true,speed);

                },sum);
            }
            else
            {
                entity.SetSkillMoveState(true,speed);
            }

            sum += skillMoveData.moveTime;
            timerSvc.AddTimeTask(i =>
            {
                entity.SetSkillMoveState(false);
            
            },  sum);
        }

    }

}