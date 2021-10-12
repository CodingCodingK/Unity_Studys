public class StateBorn : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.curtAniState = AniState.Born;
    }

    public void Process(EntityBase entity, params object[] args)
    {
        // 播放出生动画
        entity.SetAction(Constants.ActionBorn);
        TimerSvc.Instance().AddTimeTask(i =>
        {
            entity.SetAction(Constants.ActionDefault);
        }, 500);
    }

    public void Exit(EntityBase entity, params object[] args)
    {
       
    }
}