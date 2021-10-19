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
using PENet;
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
    
    public void SkillAttack(EntityBase entity,int skillID)
    {
        entity.skMoveCBLst.Clear();
        entity.skActionCBLst.Clear();
        
        AttackDamage(entity,skillID);
        AttackEffect(entity,skillID);
    }

    /// <summary>
    /// 技能效果表现
    /// </summary>
    private void AttackEffect(EntityBase entity,int skillID)
    {
        SkillCfg skillData = resSvc.GetSkillCfgData(skillID);

        if (!skillData.isCollide)
        {
            // 忽略玩家与怪物之间的碰撞
            Physics.IgnoreLayerCollision(9,10);
            timerSvc.AddTimeTask(i =>
            {
                Physics.IgnoreLayerCollision(9,10,false);
            },skillData.skillTime);
        }

        // 释放技能时移动
        if (entity.entityType == EntityType.Player)
        {
            if (entity.GetDirInput() == Vector2.zero)
            {
                // TODO 搜索最近怪物
                Vector2 dir = entity.CalcTargetDir();
                if (dir != Vector2.zero)
                {
                    entity.SetAtkRotation(dir);
                }
            }
            else
            {
                entity.SetAtkRotation(entity.GetDirInput(),true);
            }
        }

        // 设置技能 动作编号
        entity.SetAction(skillData.aniAction);
        // 设置技能特效 特效名、特效开启持续时间
        entity.SetFX(skillData.fx,skillData.skillTime);

        // 设置技能位移 适配多段位移、技能位移延迟
        CalcSkillMove(entity, skillData);

        entity.canControl = false;
        entity.SetDir(Vector2.zero);

        if (!skillData.isBreak)
        {
            entity.entityState = EntityState.BaTiState;
        }
        
        // 技能时间结束后 设置状态归零 
        entity.skEndCB = timerSvc.AddTimeTask(i =>
        {
            entity.Idle();
        }, skillData.skillTime);
    }

    /// <summary>
    /// 技能伤害计算
    /// </summary>
    private void AttackDamage(EntityBase entity,int skillID)
    {
        SkillCfg skillData = resSvc.GetSkillCfgData(skillID);
        List<int> skillActionLst = skillData.skillActionLst;
        int sum = 0;
        for (var index = 0; index < skillActionLst.Count; index++)
        {
            int tmpIndex = index;
            var actionId = skillActionLst[index];
            var action = resSvc.GetSkillActionCfgData(actionId);
            sum += action.delayTime;
            if (sum > 0)
            {
                
                int actId = timerSvc.AddTimeTask(i =>
                {
                    if (entity != null)
                    {
                        SkillAction(entity, skillData, tmpIndex); 
                        entity.RemoveActionCB(i);
                    }
                }, sum);
                entity.skActionCBLst.Add(actId);
            }
            else
            {
                SkillAction(entity, skillData, tmpIndex);
            }
        }
    }

    /// <summary>
    /// 技能 点伤害计算
    /// </summary>
    private void SkillAction(EntityBase caster,SkillCfg skill,int index)
    {
        // 获取所有怪物实体，遍历运算
        var monsterList = caster.battleMgr.GetEntityMonsters();
        if (skill.skillActionLst.Count <= index)
        {
            Debug.Log("Index:" + index + ",skillActionLst:" + skill.skillActionLst.Count + ",data:" + skill.skillActionLst);
            return;
        }
        var action = resSvc.GetSkillActionCfgData(skill.skillActionLst[index]);
        var damage = skill.skillDamageLst[index];

        if (caster.entityType == EntityType.Monster)
        {
            var targetPlayer = caster.battleMgr.entityPlayer;
            if (targetPlayer == null)
            {
                return;
            }
            
            // 判断距离、角度
            if (InRange(caster.GetPos(),targetPlayer.GetPos(),action.radius) && InAngle(caster.GetTrans(),targetPlayer.GetPos(),action.angle))
            {
                // 计算伤害
                CalcDamage(caster,targetPlayer,skill,damage);
            }
        }
        else if (caster.entityType == EntityType.Player)
        {
            foreach (var monster in monsterList)
            {
                // 判断距离、角度
                if (InRange(caster.GetPos(),monster.GetPos(),action.radius) && InAngle(caster.GetTrans(),monster.GetPos(),action.angle))
                {
                    // 计算伤害
                    CalcDamage(caster,monster,skill,damage);
                }
            }
        }
       
    }

    private bool InRange(Vector3 from,Vector3 to,float range)
    {
        float dis = Vector3.Distance(from, to);
        return dis <= range;
    }
    
    private bool InAngle(Transform from,Vector3 to,float angle)
    {
        if (angle >= 360)
        {
            return true;
        }

        Vector3 start = from.forward;
        Vector3 dir = (to - from.position).normalized;
        float curtAngle = Vector3.Angle(start, dir);
        // 主角朝向的线与主角点-怪物点连线画一个角，如果这个角小于技能的角度的1/2（技能角度是主角朝向的扇形角），则判定为怪物被技能角度包括在内
        return curtAngle <= angle / 2f;
    }

    private System.Random rd = new System.Random();
    private void CalcDamage(EntityBase caster,EntityBase target,SkillCfg skillCfg,int damage)
    {
        int dmgSum = damage;
        if (DamageType.AD.Equals(skillCfg.dmgType))
        {
            // 计算闪避
            int dodgeNum = PETools.RDInt(1, 100, rd);
            if (dodgeNum <= target.Props.dodge)
            {
                // TODO UI显示闪避
                target.SetDodge();
                return;
            }
            // 计算属性加成（人物固有伤害+技能额外伤害）
            dmgSum += caster.Props.ad;

            // 计算暴击
            int criticalNum = PETools.RDInt(1, 100, rd);
            if (criticalNum <= target.Props.critical) ;
            {
                // 暴击伤害 = 2倍
                var criticalRate = 2;
                dmgSum = dmgSum * criticalRate;
                target.SetCritical(dmgSum);
            }

            // 计算穿甲
            int addef = (int) ((1 - caster.Props.pierce / 100f) * target.Props.addef);
            dmgSum -= addef;
        }
        else
        {
            // 计算属性加成（人物固有伤害+技能额外伤害）
            dmgSum += caster.Props.ap;
            // 计算魔抗
            dmgSum -= target.Props.apdef;
        }
        
        // 最终伤害
        dmgSum = dmgSum >= 0 ? dmgSum : 0;
        if (dmgSum == 0)
        {
            return;
        }
        
        Debug.Log("Damage:" + dmgSum + ",Hp:" + target.HP);

        target.SetHurt(dmgSum);
        if (target.HP < dmgSum)
        {
            target.HP = 0;
            // TODO 目标死亡处理
            target.Die();
            if (target.entityType == EntityType.Monster)
            {
                BattleMgr.Instance.RemoveMonster(target.Name);
            }
            else if (target.entityType == EntityType.Player)
            {
                target.battleMgr.entityPlayer = null;
                target.battleMgr.EndBattle(false,0);
            }
            
        }
        else
        {
            target.HP -= dmgSum;
            if (target.entityState != EntityState.BaTiState && target.GetBreakState())
            {
                target.Hit();
            }
        }
        
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
                int moveId = timerSvc.AddTimeTask(i =>
                {
                    entity.SetSkillMoveState(true,speed);
                    entity.RemoveMoveCB(i);
                },sum);
                entity.skMoveCBLst.Add(moveId);
            }
            else
            {
                entity.SetSkillMoveState(true,speed);
            }

            sum += skillMoveData.moveTime;
            int stopId = timerSvc.AddTimeTask(i =>
            {
                entity.SetSkillMoveState(false);
                entity.RemoveMoveCB(i);
            },  sum);
            entity.skMoveCBLst.Add(stopId);
        }

    }

}