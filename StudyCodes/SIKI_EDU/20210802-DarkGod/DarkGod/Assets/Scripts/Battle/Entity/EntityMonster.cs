public class EntityMonster : EntityBase
{
    
    public MonsterData md;
    
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
}