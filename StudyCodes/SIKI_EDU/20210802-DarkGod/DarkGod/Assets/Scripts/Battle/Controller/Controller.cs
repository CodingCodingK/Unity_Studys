
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 表现实体控制器基类
/// </summary>
public abstract class Controller : MonoBehaviour
{
    public Animator ani;

    protected bool isMove = false;
    private Vector2 dir = Vector2.zero;
    public Vector2 Dir
    {
        get
        {
            return dir;
        }
        set
        {
            if (value == Vector2.zero)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
                dir = value;
            }
        }
    }

    protected TimerSvc timerSvc;
    protected Dictionary<string, GameObject> fxDic = new Dictionary<string, GameObject>();

    protected bool skillMove = false;
    protected float skillMoveSpeed = 0f;
    
    public virtual void Init()
    {
        timerSvc = TimerSvc.Instance();
    }
    
    public virtual void SetBlend(float blend)
    {
        ani.SetFloat("Blend",blend);
    }

    public virtual void SetAction(int act)
    {
        ani.SetInteger("Action",act);
    }
    
    public virtual void SetFX(string name, float destroy)
    {
        
    }

    public void SetSkillMove(bool move,float skillSpeed = 0f)
    {
        skillMove = move;
        skillMoveSpeed = skillSpeed;
    }
}

