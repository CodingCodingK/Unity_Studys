/// 逻辑实体基类
public class EntityBase
{
    public AniState curtAniState;
    public StateMgr stateMgr;
    public Controller controller;

    public void Move()
    {
        stateMgr.ChangeStatus(this,curtAniState = AniState.Move);
    }
    
    public void Idle()
    {
        stateMgr.ChangeStatus(this,curtAniState = AniState.Idle);
    }

    public virtual void SetBlend(float blend)
    {
        if (controller!=null)
        {
            controller.SetBlend(blend);
        }
    }
}