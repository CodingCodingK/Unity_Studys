
public class StateMove : IState
{
    public void Enter(EntityBase entity,params object[] args)
    {
        entity.curtAniState = AniState.Move;
    }

    public void Process(EntityBase entity,params object[] args)
    {
        entity.SetBlend(Constants.BlendMove);
    }

    public void Exit(EntityBase entity,params object[] args)
    {
       
    }
}