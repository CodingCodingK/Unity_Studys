

using UnityEngine;

public class StateIdle : IState
{
    public void Enter(EntityBase entity,params object[] args)
    {
        entity.curtAniState = AniState.Idle;
    }

    public void Process(EntityBase entity,params object[] args)
    {
        entity.SetBlend(Constants.BlendIdle);
        var dir = entity.GetDirInput();
        if (dir != Vector2.zero)
        {
            entity.SetDir(dir);
            entity.Move();
        }
        else
        {
            entity.Idle();
        }
    }

    public void Exit(EntityBase entity,params object[] args)
    {
        
    }
}