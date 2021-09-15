/****************************************************
    文件：PlayerController.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/15 22:34:38
    功能：角色控制器
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Net.Sockets;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Main Camera的引用
    /// </summary>
    private Transform camTrans;
    private Vector3 camOffset;
    public Animator ani;
    public CharacterController ctrl;
    
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

    private bool isMove = false;

    private void Start()
    {
        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;

    }
    
    private void Update()
    {
        #region Input
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 _dir = new Vector2(h, v).normalized;
        Dir = _dir;
        

        #endregion

        if (isMove)
        {
            //设置方向
            SetDir();
            //产生移动
            SetMove();
            //相机跟随
            SetCam();
        }
        
    }

    private void SetDir()
    {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1));
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
    }
    
    private void SetCam()
    {
        if (camTrans != null)
        {
            camTrans.position = transform.position - camOffset;
        }
    }
}
