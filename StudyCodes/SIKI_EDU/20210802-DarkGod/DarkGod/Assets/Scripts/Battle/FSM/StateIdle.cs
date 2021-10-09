

public class StateIdle : IState
{
    public void Enter(EntityBase entity)
    {
        entity.curtAniState = AniState.Idle;
    }

    public void Process(EntityBase entity)
    {
        entity.SetBlend(Constants.BlendIdle);
    }

    public void Exit(EntityBase entity)
    {
        
    }
}