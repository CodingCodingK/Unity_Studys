/****************************************************
    文件：TestPlayer.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/10/10 15:31:11
    功能：测试控制玩家脚本
*****************************************************/

using System.Collections;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public Animator ani;

    protected bool isMove = false;
    private Vector2 dir = Vector2.zero;
    public GameObject daggerskill1fx;
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

    /// <summary>
    /// Main Camera的引用
    /// </summary>
    private Transform camTrans;
    private Vector3 camOffset;
    public CharacterController ctrl;
    
    private float targetBlend;
    private float currentBlend;

    public void Start()
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
        if (_dir != Vector2.zero)
        {
            SetBlend(Constants.BlendMove);
        }
        else
        {
            SetBlend(Constants.BlendIdle);
        }
       
        Dir = _dir;
         
        #endregion
        
        if (currentBlend != targetBlend)
        {
            UpdateMixBlend();
        }

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
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1)) + camTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
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

    public void SetBlend(float blend)
    {
        targetBlend = blend;
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

    #region Test

    public void ClickSkill1()
    {
        ani.SetInteger("Action",1);
        daggerskill1fx.gameObject.SetActive(true);
        StartCoroutine(Delay());
    }

    #endregion

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        ani.SetInteger("Action",-1);
        daggerskill1fx.gameObject.SetActive(false);
    }
}
