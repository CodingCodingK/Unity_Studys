using UnityEngine;

/// 逻辑实体基类
public class EntityBase
{
    public AniState curtAniState;
    public StateMgr stateMgr;
    public SkillMgr skillMgr;
    public Controller controller;

    public void Move()
    {
        stateMgr.ChangeStatus(this,AniState.Move);
    }
    
    public void Idle()
    {
        stateMgr.ChangeStatus(this,AniState.Idle);
    }
    
    public void Attack(int skillID)
    {
        stateMgr.ChangeStatus(this,AniState.Attack,skillID);
    }
    
    public virtual void SetBlend(float blend)
    {
        if (controller!=null)
        {
            controller.SetBlend(blend);
            Debug.Log("!!! "+ blend);
        }
    }
    
    public virtual void SetDir(Vector2 dir)
    {
        if (controller!=null)
        {
            controller.Dir = dir;
        }
    }
    
    public virtual void SetAction(int act)
    {
        if (controller!=null)
        {
            controller.SetAction(act);
        }
    }
    
    public virtual void SetFX(string name, float destroy)
    {
        if (controller!=null)
        {
            controller.SetFX(name,destroy);
        }
    }

    public virtual void AttackEffect(int skillID)
    {
        skillMgr.AttackEffect(this,skillID);
    }
}