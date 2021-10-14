using UnityEngine;

/// 逻辑实体基类
public class EntityBase
{
    public AniState curtAniState;
    public StateMgr stateMgr;
    public SkillMgr skillMgr;
    public BattleMgr battleMgr;
    public Controller controller;

    public string Name;

    public bool canControl = true;

   
    private BattleProps _props;
    
    /// <summary>
    /// 战斗状态
    /// </summary>
    public BattleProps Props
    {
        get
        {
            return _props;
        }
        protected set
        {
            _props = value;
        }
    }
    
    private int _hp;
    public int HP
    {
        get
        {
            return _hp;
        }
        set
        {
            // 更新UI层
            SetHpVal(_hp,value);
            _hp = value;
        }
    }

    public virtual void SetBattleProps(BattleProps bps)
    {
        Props = bps;
        HP = _props.hp;
    }

    public void Move()
    {
        stateMgr.ChangeStatus(this,AniState.Move);
    }
    
    public void Born()
    {
        stateMgr.ChangeStatus(this,AniState.Born);
    }
    
    public void Idle()
    {
        stateMgr.ChangeStatus(this,AniState.Idle);
    }
    
    public void Attack(int skillID)
    {
        stateMgr.ChangeStatus(this,AniState.Attack,skillID);
    }
    
    public void Hit()
    {
        stateMgr.ChangeStatus(this,AniState.Hit);
    }
    
    public void Die()
    {
        stateMgr.ChangeStatus(this,AniState.Die);
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
    
    public virtual void SetSkillMoveState(bool move,float skillSpeed = 0f)
    {
        if (controller!=null)
        {
            controller.SetSkillMoveState(move,skillSpeed);
        }
    }

    public virtual void SetDodge()
    {
        GameRootResources.Instance().dynamicWindow.SetDodge(controller.gameObject.name);
    }
    
    public virtual void SetCritical(int num)
    {
        GameRootResources.Instance().dynamicWindow.SetCritical(controller.gameObject.name,num);
    }
    
    public virtual void SetHurt(int num)
    {
        GameRootResources.Instance().dynamicWindow.SetHurt(controller.gameObject.name,num);
    }
    public virtual void SetHpVal(int oldVal,int newVal)
    {
        GameRootResources.Instance().dynamicWindow.SetHpVal(controller.gameObject.name,oldVal,newVal);
    }

    public virtual void SkillAttack(int skillID)
    {
        skillMgr.SkillAttack(this,skillID);
    }
    
    // public virtual void AttackDamage(int skillID)
    // {
    //     skillMgr.AttackDamage(this,skillID);
    // }

    public virtual Vector2 GetDirInput()
    {
        return Vector2.zero;
    }
    
    public virtual Vector3 GetPos()
    {
        return controller.transform.position;
    }
    
    public virtual Transform GetTrans()
    {
        return controller.transform;
    }
}