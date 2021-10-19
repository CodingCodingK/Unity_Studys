

using UnityEngine;

public class StateIdle : IState
{
    public void Enter(EntityBase entity,params object[] args)
    {
        entity.curtAniState = AniState.Idle;
        entity.SetDir(Vector2.zero);
        entity.skEndCB = -1;
    }

    public void Process(EntityBase entity,params object[] args)
    {
        if (entity.nextSkillID != 0)
        {
            entity.Attack(entity.nextSkillID);
        }
        else
        {
            if (entity.entityType == EntityType.Player)
            {
                entity.canSkill = true;
            }
            
            var dir = entity.GetDirInput();
            if (dir != Vector2.zero)
            {
                entity.SetDir(dir);
                entity.Move();
            }
            else
            {
                entity.SetBlend(Constants.BlendIdle);
            }
        }

    }

    public void Exit(EntityBase entity,params object[] args)
    {
        
    }
}