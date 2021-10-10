

public class StateIdle : IState
{
    public void Enter(EntityBase entity,params object[] args)
    {
        entity.curtAniState = AniState.Idle;
    }

    public void Process(EntityBase entity,params object[] args)
    {
        entity.SetBlend(Constants.BlendIdle);
    }

    public void Exit(EntityBase entity,params object[] args)
    {
        
    }
}