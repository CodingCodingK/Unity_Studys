using System;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 血条物体
/// </summary>
public class ItemEntityHp : MonoBehaviour
{
    #region UI Define

    public Image imgHPGray;
    public Image imgHPRed;

    public Animation criticalAni;
    public Text txtCritical;
    
    public Animation dodgeAni;
    public Text txtDodge;
    
    public Animation damageAni;
    public Text txtDamage;

    /// <summary>
    /// 怪物位置，用于血条跟踪
    /// </summary>
    private Transform rootTrans;
    private RectTransform rect;
    
    #endregion

    #region Other Define

    private int hpVal;
    private float scaleRate = 1f * Constants.ScreenStandardHeight / Screen.height;
    #endregion

    public void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(rootTrans.position);
        rect.anchoredPosition = screenPos * scaleRate;
        
        UpdateMixBlend();
        imgHPGray.fillAmount = curtPrg;
    }

    public void SetItemInfo(Transform trans,int hp)
    {
        rect = transform.GetComponent<RectTransform>();
        rootTrans = trans;
        hpVal = hp;
        imgHPGray.fillAmount = 1;
        imgHPRed.fillAmount = 1;
    }
    

    public void SetCritical(int num)
    {
        criticalAni.Stop();
        txtCritical.text = "暴击 " + num;
        criticalAni.Play();
    }
    
    public void SetDodge()
    {
        dodgeAni.Stop();
        dodgeAni.Play();
    }
    
    public void SetHurt(int num)
    {
        damageAni.Stop();
        txtDamage.text = "-" + num;
        damageAni.Play();
    }

    private float curtPrg = 1f;
    private float targetPrg = 1f;
    public void SetHpVal(int oldVal,int newVal)
    {
        curtPrg = oldVal * 1f / hpVal;
        targetPrg = newVal * 1f / hpVal;
        imgHPRed.fillAmount = targetPrg;
    }
    
    /// <summary>
    /// 平滑处理血条跟随
    /// </summary>
    private void UpdateMixBlend()
    {
        // 变化控制在规定的加速度最大值之内
        if (Mathf.Abs(curtPrg - targetPrg) < Constants.AccelerHpSpeed * Time.deltaTime)
        {
            curtPrg = targetPrg;
        }
        else if (curtPrg > targetPrg)
        {
            curtPrg -= Constants.AccelerHpSpeed * Time.deltaTime;
        }
        else
        {
            curtPrg += Constants.AccelerHpSpeed * Time.deltaTime;
        }
        
    }
}