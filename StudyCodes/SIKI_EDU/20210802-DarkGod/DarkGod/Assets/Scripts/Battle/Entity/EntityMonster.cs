using System.Timers;
using PENet;
using UnityEngine;

public class EntityMonster : EntityBase
{
    public EntityMonster()
    {
        entityType = EntityType.Monster;
    }
    
    public MonsterData md;

    private float checkTime = 2;
    private float checkCountTime = 0;
    private float atkTime = 2;
    private float atkCountTime = 0;
    private bool runAI = true;
    
    /// <summary>
    /// 属性与等级挂钩，因此重写
    /// </summary>
    public override void SetBattleProps(BattleProps bps)
    {
        int lv = md.mLevel;
        // 游戏公式：怪物属性 = 怪物lv * 怪物1级属性
        var p = new BattleProps()
        {
            hp = bps.hp * lv,
            ad = bps.ad * lv,
            ap = bps.ap * lv,
            addef = bps.addef * lv,
            apdef = bps.apdef * lv,
            dodge = bps.dodge * lv,
            pierce = bps.pierce * lv,
            critical = bps.critical * lv,
        };

        base.SetBattleProps(p);
    }
    
    public override void TickAILogic()
    {
        if (!runAI || !(curtAniState == AniState.Idle || curtAniState == AniState.Move))
        {
            return;
        }
        
        float delta = Time.deltaTime;
        checkCountTime += delta;
        if (checkCountTime < checkTime)
        {
            return;
        }
        else
        {
            // 计算目标方向
            Vector2 dir = CalcTargetDir();
         
            if (!InAtkRange())
            {
                // 攻击范围外进行移动
                SetDir(dir);
                Move();
            }
            else
            {
                // 攻击范围内进行攻击，判断攻击间隔
                SetDir(Vector2.zero);
                atkCountTime += checkCountTime;
                if (atkCountTime > atkTime)
                {
                    SetAtkRotation(dir);
                    Attack(md.mCfg.skillID);
                    atkCountTime = 0;
                }
                else
                {
                    Idle();
                }
            }

            checkCountTime = 0;
            // 随机给予反应时间，造成怪物的个体差异
            checkTime = PETools.RDInt(1,5) * 1.0f / 10;
        }
    }

    public override Vector2 CalcTargetDir()
    {
        EntityPlayer player = battleMgr.entityPlayer;
        if (player==null || player.curtAniState == AniState.Die)
        {
            runAI = false;
            return Vector2.zero;
        }
        else
        {
            Vector3 target = player.GetPos();
            Vector3 self = GetPos();
            return new Vector2(target.x - self.x, target.z - self.z).normalized;
        }
    }

    private bool InAtkRange()
    {
        EntityPlayer entityPlayer = battleMgr.entityPlayer;
        if (entityPlayer == null || entityPlayer.curtAniState == AniState.Die)
        {
            runAI = false;
            return false;
        }
        else
        {
            Vector3 target = entityPlayer.GetPos();
            Vector3 self = GetPos();
            target.y = 0;
            self.y = 0;
            float dis = Vector3.Distance(target, self);
            if (dis <= md.mCfg.atkDis)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    
    public override bool GetBreakState()
    {
        if (md.mCfg.isStop)
        {
            // 如果怪物是可以被打断的，就判断目前技能是否能被打断
            if (curtSkillCfg != null && curtAniState == AniState.Attack)
            {
                return curtSkillCfg.isBreak;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }
}
