public class StateAttack : IState
{
    public void Enter(EntityBase entity,params object[] args)
    {
        entity.curtAniState = AniState.Attack;
    }

    public void Process(EntityBase entity,params object[] args)
    {
        entity.AttackDamage((int)args[0]);
        entity.AttackEffect((int)args[0]);
    }

    public void Exit(EntityBase entity,params object[] args)
    {
        // 技能表现重置
        entity.SetAction(Constants.ActionDefault);
        // 允许移动
        entity.canControl = true;
    }
}