public class StateDie : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.curtAniState = AniState.Die;
    }

    public void Process(EntityBase entity, params object[] args)
    {
        entity.SetAction(Constants.ActionDie);
        TimerSvc.Instance().AddTimeTask(i =>
        {
            entity.controller.gameObject.SetActive(false);
        },Constants.DieAniTime);
    }

    public void Exit(EntityBase entity, params object[] args)
    {
       
    }
}