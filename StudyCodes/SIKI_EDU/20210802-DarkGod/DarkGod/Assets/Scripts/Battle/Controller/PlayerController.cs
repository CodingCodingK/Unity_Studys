/****************************************************
    文件：PlayerController.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/15 22:34:38
    功能：表现实体角色控制器
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Net.Sockets;
using UnityEngine;

public class PlayerController : Controller
{
    private Vector3 camOffset;

    private float targetBlend;
    private float currentBlend;
    
    // 技能
    public GameObject daggerAtk1fx;
    public GameObject daggerAtk2fx;
    public GameObject daggerAtk3fx;
    public GameObject daggerAtk4fx;
    public GameObject daggerAtk5fx;
    public GameObject daggerSkill1fx;
    public GameObject daggerSkill2fx;
    public GameObject daggerSkill3fx;

    public override void Init()
    {
        base.Init();
        
        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;

        // 加入所有技能特效
        if (daggerAtk1fx != null)
        {
            fxDic.Add(daggerAtk1fx.name,daggerAtk1fx);
        }
        if (daggerAtk2fx != null)
        {
            fxDic.Add(daggerAtk2fx.name,daggerAtk2fx);
        }
        if (daggerAtk3fx != null)
        {
            fxDic.Add(daggerAtk3fx.name,daggerAtk3fx);
        }
        if (daggerAtk4fx != null)
        {
            fxDic.Add(daggerAtk4fx.name,daggerAtk4fx);
        }
        if (daggerAtk5fx != null)
        {
            fxDic.Add(daggerAtk5fx.name,daggerAtk5fx);
        }
        if (daggerSkill1fx != null)
        {
            fxDic.Add(daggerSkill1fx.name,daggerSkill1fx);
        }
        if (daggerSkill2fx != null)
        {
            fxDic.Add(daggerSkill2fx.name,daggerSkill2fx);
        }
        if (daggerSkill3fx != null)
        {
            fxDic.Add(daggerSkill3fx.name,daggerSkill3fx);
        }
    }
    
    private void Update()
    {
        #region Input
        
        /*
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 _dir = new Vector2(h, v).normalized;
        if (_dir != Vector2.zero)
        {
            SetBlend(Constants.BlendWalk);
        }
        else
        {
            SetBlend(Constants.BlendIdle);
        }
       
        Dir = _dir;
         */
        #endregion
        
        if (currentBlend != targetBlend)
        {
            UpdateMixBlend();
        }

        // 摇杆移动
        if (isMove)
        {
            //设置方向
            SetDir();
            //产生移动
            SetMove();
            //相机跟随
            SetCam();
        }

        // 技能移动
        if (skillMove)
        {
            //产生移动
            SetSkillMove();
            //相机跟随
            SetCam();
        }
    }

    private void SetDir()
    {
        // Dir = 从托盘上计算出来的偏转角度（中点到用户指触点）
        
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1)) + camTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
    }
    
    private void SetSkillMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * skillMoveSpeed);
    }
    
    /// <summary>
    /// 更新主摄像机，跟随主角
    /// </summary>
    public void SetCam()
    {
        if (camTrans != null)
        {
            camTrans.position = transform.position - camOffset;
        }
    }

    public override void SetBlend(float blend)
    {
        targetBlend = blend;
    }
    
    public override void SetFX(string name, float destroy)
    {
        if (fxDic.TryGetValue(name,out GameObject go))
        {
            go.SetActive(true);
            timerSvc.AddTimeTask(i =>
            {
                go.SetActive(false);
            }, destroy, PETimeUnit.Millisecond, 1);
            
        }
    }

    
    private void UpdateMixBlend(float blend)
    {
        ani.SetFloat("Blend",blend);
    }

    /// <summary>
    /// 平滑处理角色状态变换
    /// </summary>
    private void UpdateMixBlend()
    {
        // blender的变化控制在规定的加速度最大值之内
        if (Mathf.Abs(currentBlend - targetBlend) < Constants.AccelerSpeed * Time.deltaTime)
        {
            currentBlend = targetBlend;
        }
        else if (currentBlend > targetBlend)
        {
            currentBlend -= Constants.AccelerSpeed * Time.deltaTime;
        }
        else
        {
            currentBlend += Constants.AccelerSpeed * Time.deltaTime;
        }

        UpdateMixBlend(currentBlend);
    }

}
