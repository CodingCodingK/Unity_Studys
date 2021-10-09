
using UnityEngine;

/// <summary>
/// 表现实体控制器基类
/// </summary>
public abstract class Controller : MonoBehaviour
{
    public Animator ani;
    public virtual void SetBlend(float blend)
    {
        ani.SetFloat("Blend",blend);
    }
}

