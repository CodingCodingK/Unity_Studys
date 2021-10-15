using System.Timers;
using UnityEngine;

public class EntityMonster : EntityBase
{
    
    public MonsterData md;

    private float checkTime = 2;
    private float checkCountTime = 0;
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
        float delta = Time.deltaTime;
        checkCountTime += delta;
        if (checkCountTime < checkTime)
        {
            return;
        }
        else
        {
            // TODO 计算目标方向
            Vector2 dir = 
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
}