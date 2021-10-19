using System.Collections.Generic;
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
    public EntityType entityType = EntityType.None;
    public EntityState entityState = EntityState.None;

    public bool canControl = true;
    public bool canSkill = true;

    /// 技能位移的回调ID
    public List<int> skMoveCBLst = new List<int>();
    
    /// 技能伤害的回调ID
    public List<int> skActionCBLst = new List<int>();
   
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

    public Queue<int> comboQue = new Queue<int>();
    public int nextSkillID;
    public SkillCfg curtSkillCfg;
    public int skEndCB = -1;

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

    public virtual void TickAILogic()
    {
        
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

    public virtual void SetAtkRotation(Vector2 dir,bool offset = false)
    {
        if (controller!=null)
        {
            if (offset)
            {
                controller.SetAtkRotation(dir);
            }
            else
            {
                controller.SetAtkRotationLocal(dir);
            }
            
        }
    }

    public virtual void SetDodge()
    {
        GameRootResources.Instance().dynamicWindow.SetDodge(Name);
    }
    
    public virtual void SetCritical(int num)
    {
        GameRootResources.Instance().dynamicWindow.SetCritical(Name,num);
    }
    
    public virtual void SetHurt(int num)
    {
        GameRootResources.Instance().dynamicWindow.SetHurt(Name,num);
    }
    public virtual void SetHpVal(int oldVal,int newVal)
    {
        GameRootResources.Instance().dynamicWindow.SetHpVal(Name,oldVal,newVal);
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

    public AnimationClip[] GetAniClips()
    {
        return controller.ani.runtimeAnimatorController.animationClips;
    }
    public AudioSource GetAudioSource()
    {
        return controller.GetComponent<AudioSource>();
    }
    

    public virtual Vector2 CalcTargetDir()
    {
        return Vector2.zero;
    }

    public void ExitCurtSkill()
    {
        canControl = true;
        entityState = EntityState.None;

        // 连招数据更新
        if (curtSkillCfg.isCombo)
        {
            if (comboQue.Count > 0)
            {
                nextSkillID = comboQue.Dequeue();
            }
            else
            {
                nextSkillID = 0;
            }
        }

        curtSkillCfg = null;
        SetAction(Constants.ActionDefault);
    }

    public void RemoveMoveCB(int tid)
    {
        int index = -1;
        for (int i = 0; i < skMoveCBLst.Count; i++)
        {
            if (skMoveCBLst[i] == tid)
            {
                index = i;
                break;
            }
        }

        if (index != -1)
        {
            skMoveCBLst.RemoveAt(index);
        }
    }
    
    public void RemoveActionCB(int tid)
    {
        int index = -1;
        for (int i = 0; i < skActionCBLst.Count; i++)
        {
            if (skActionCBLst[i] == tid)
            {
                index = i;
                break;
            }
        }

        if (index != -1)
        {
            skActionCBLst.RemoveAt(index);
        }
    }

    /// <summary>
    /// 当前是否可以被中断
    /// </summary>
    public virtual bool GetBreakState()
    {
        return true;
    }

    /// <summary>
    /// 去除技能回调，取消回调后续动画播放与伤害计算。
    /// </summary>
    public void RemoveSkillCB()
    { 
        SetDir(Vector2.zero);
        SetSkillMoveState(false);
        
        foreach (var move in skMoveCBLst)
        {
            TimerSvc.Instance().DelTask(move);
        }
                
        foreach (var action in skActionCBLst)
        {
            TimerSvc.Instance().DelTask(action);
        }
                
        // 攻击被中断，删除定时回调的Idle
        if (skEndCB != -1)
        {
            TimerSvc.Instance().DelTask(skEndCB);
            skEndCB = -1;
        }
                
        // 被击，清空连招
        if (nextSkillID != 0 || comboQue.Count > 0)
        {
            nextSkillID = 0;
            comboQue.Clear();
        
            battleMgr.lastAtkTime = 0;
            battleMgr.comboIndex = 0;
        
            skMoveCBLst.Clear();
            skActionCBLst.Clear();
        }
    }
}