using UnityEngine;

public class StateHit : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.curtAniState = AniState.Hit;

        foreach (var move in entity.skMoveCBLst)
        {
            TimerSvc.Instance().DelTask(move);
        }
        
        foreach (var action in entity.skActionCBLst)
        {
            TimerSvc.Instance().DelTask(action);
        }
    }

    public void Process(EntityBase entity, params object[] args)
    {
        if (entity.entityType == EntityType.Player)
        {
            entity.canSkill = false;
        }
        
        entity.SetDir(Vector2.zero);
        entity.SetAction(Constants.ActionHit);

        if (entity.entityType == EntityType.Player)
        {
            var charAudioSource = entity.GetAudioSource();
            AudioSvc.Instance().PlayCustomClip(Constants.AssHit,charAudioSource);
        }
        
         // 受伤后1秒进入Idle状态
         TimerSvc.Instance().AddTimeTask(o =>
         {
             entity.SetAction(Constants.ActionDefault);
             entity.Idle();
         }, (int)(GetHitAniTIme(entity) * 1000));
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        
    }

    /// <summary>
    /// 获取受击动画时长,单位:秒
    /// </summary>
    private float GetHitAniTIme(EntityBase entity)
    {
        AnimationClip[] clips = entity.GetAniClips();
        foreach (var clip in clips)
        {
            if (clip.name.ToLower().Contains("hit"))
            {
                return clip.length;
            }
        }
        // 找不到hit动画片段时,给的值
        return 1;
    }
}