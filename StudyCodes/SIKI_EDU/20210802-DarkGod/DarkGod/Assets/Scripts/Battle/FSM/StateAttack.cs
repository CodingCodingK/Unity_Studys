public class StateAttack : IState
{
    public void Enter(EntityBase entity,params object[] args)
    {
        entity.curtAniState = AniState.Attack;
        entity.curtSkillCfg = ResSvc.Instance().GetSkillCfgData((int)args[0]);
    }

    public void Process(EntityBase entity,params object[] args)
    {
        if (entity.entityType == EntityType.Player)
        {
            entity.canSkill = false;
        }
        entity.SkillAttack((int)args[0]);
    }

    public void Exit(EntityBase entity,params object[] args)
    {
        entity.ExitCurtSkill();
    }
}