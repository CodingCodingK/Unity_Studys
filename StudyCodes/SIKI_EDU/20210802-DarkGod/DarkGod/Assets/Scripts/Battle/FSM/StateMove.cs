
public class StateMove : IState
{
    public void Enter(EntityBase entity)
    {
        entity.curtAniState = AniState.Move;
    }

    public void Process(EntityBase entity)
    {
        entity.SetBlend(Constants.BlendMove);
    }

    public void Exit(EntityBase entity)
    {
       
    }
}