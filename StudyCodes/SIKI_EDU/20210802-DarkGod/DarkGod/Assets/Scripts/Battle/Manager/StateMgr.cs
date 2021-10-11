/****************************************************
    文件：StateMgr.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/10/7 21:49:17
    功能：状态切换管理器
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PEProtocol;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateMgr: MonoBehaviour
{
    /// <summary>
    /// 所有动画对应动画状态机字典
    /// </summary>
    private Dictionary<AniState, IState> fsmDic = new Dictionary<AniState, IState>();
    
    public void Init()
    {
        fsmDic.Add(AniState.Idle,new StateIdle());
        fsmDic.Add(AniState.Move,new StateMove());
        fsmDic.Add(AniState.Attack,new StateAttack());
        
        Debug.Log("Init StateMgr.");
    }

    public void ChangeStatus(EntityBase entity,AniState targetState,params object[] args)
    {
        if (entity.curtAniState == targetState)
        {
            return;
        }
        
        if (fsmDic.ContainsKey(targetState))
        {
            if (entity.curtAniState != AniState.None)
            {
                fsmDic[entity.curtAniState].Exit(entity,args);
            }
            fsmDic[targetState].Enter(entity,args);
            fsmDic[targetState].Process(entity,args);
        }
    }

}