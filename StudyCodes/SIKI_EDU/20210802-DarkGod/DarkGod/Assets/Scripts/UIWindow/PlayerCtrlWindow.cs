/****************************************************
    文件：PlayerCtrlWindow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/23
    功能：玩家控制界面
*****************************************************/


using System;
using System.Collections.Generic;
using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCtrlWindow : WindowBase
{
    #region define
    // UI
    
    // 拖拽范围
    public Image imgTouch;
    // 拖拽背景盘
    public Image imgDirBg;
    // 拖拽点
    public Image imgDirPoint;
    
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;

    public Text txtSelfHp;
    public Image imgSelfHp;
    
    /// <summary>
    /// 经验进度条
    /// </summary>
    public Transform expPrgTrans;

    public Image imgSk1CD;
    public Text txtSk1CD;
    public Image imgSk2CD;
    public Text txtSk2CD;
    public Image imgSk3CD;
    public Text txtSk3CD;
    
    public Transform transBossHpBar;
    public Image imgBoosHpRed;
    public Image imgBossHpYellow;
    
    // others
    
    /// <summary>
    /// 摇杆用 拖拽起始点
    /// </summary>
    private Vector2 startPos = Vector2.zero;
    
    /// <summary>
    /// 摇杆用 操控盘归零位置
    /// </summary>
    private Vector2 defaultPos = Vector2.zero;

    /// <summary>
    /// 摇杆范围 自适应用
    /// </summary>
    private float pointDis;

    public Vector2 curtDir;

    private bool isSK1CD = false;
    private float sk1CdTime;
    private int sk1CdNum;
    private float sk1CdCount;
    
    private bool isSK2CD = false;
    private float sk2CdTime;
    private int sk2CdNum;
    private float sk2CdCount;
    
    private bool isSK3CD = false;
    private float sk3CdTime;
    private int sk3CdNum;
    private float sk3CdCount;

    private int hpMax;
    #endregion
    
    protected override void InitWindow()
    {
        base.InitWindow();
        
        defaultPos = imgDirBg.transform.position;
        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;

        hpMax = GameRoot.Instance().PlayerData.hp;
        SetText(txtSelfHp,hpMax + "/" + hpMax);
        imgSelfHp.fillAmount = 1;

        SetBossHpBarState(false);
        
        sk1CdTime = resSvc.GetSkillCfgData(101).cdTime / 1000f;
        sk2CdTime = resSvc.GetSkillCfgData(102).cdTime / 1000f;
        sk3CdTime = resSvc.GetSkillCfgData(103).cdTime / 1000f;
        RegClickEvents();
        RefreshUI();
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        
        if (isSK1CD)
        {
            sk1CdCount += delta;
            sk1CdNum = (int)(sk1CdTime - sk1CdCount);
            if (sk1CdCount >= sk1CdTime)
            {
                isSK1CD = false;
                SetActive(imgSk1CD,false);
                //SetActive(txtSk1CD,false);
                
            }
            else
            {
                imgSk1CD.fillAmount = 1 - sk1CdCount / sk1CdTime;
                SetText(txtSk1CD,sk1CdNum);
            }
        }
        
        if (isSK2CD)
        {
            sk2CdCount += delta;
            sk2CdNum = (int)(sk2CdTime - sk2CdCount);
            if (sk2CdCount >= sk2CdTime)
            {
                isSK2CD = false;
                SetActive(imgSk2CD,false);
                //SetActive(txtSk1CD,false);
                
            }
            else
            {
                imgSk2CD.fillAmount = 1 - sk2CdCount / sk2CdTime;
                SetText(txtSk2CD,sk2CdNum);
            }
        }
        
        if (isSK3CD)
        {
            sk3CdCount += delta;
            sk3CdNum = (int)(sk3CdTime - sk3CdCount);
            if (sk3CdCount >= sk3CdTime)
            {
                isSK3CD = false;
                SetActive(imgSk3CD,false);
                //SetActive(txtSk1CD,false);
                
            }
            else
            {
                imgSk3CD.fillAmount = 1 - sk3CdCount / sk3CdTime;
                SetText(txtSk3CD,sk3CdNum);
            }
        }
        
        // Boss Hp Bar
        UpdateMixBlend();
        imgBossHpYellow.fillAmount = curtPrg;
    }

    private void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance().PlayerData;
        SetText(txtLevel,pd.level);
        SetText(txtName,pd.name);
        
        //expPrg text
        int expPrgVal = (int)(pd.exp * 1.0f / PECommon.GetExpMaxValByLv(pd.level) * 100);
        SetText(txtExpPrg,expPrgVal + "%");

        //expPrg img
        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();
        float globalRate = Screen.height * 1.0f / Constants.ScreenStandardHeight;
        float screenWidth = Screen.width * globalRate;
        float width = (screenWidth - 180) / 10;
        
        // 高固定，宽计算适配屏幕整体的UGUI宽度自适应
        grid.cellSize = new Vector2(width, 7);
        
        // 遍历修改经验条的10个区块
        int index = expPrgVal / 10;
        for (int i = 0;i < expPrgTrans.childCount;i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                img.fillAmount = 1;
            }
            else if (i == index)
            {
                img.fillAmount = expPrgVal % 10 * 1.0f / 10;
            }
            else
            {
                img.fillAmount = 0;
            }
        }
        
        
    }

    public void SetSelfHpBarVal(int curtHp)
    {
        SetText(txtSelfHp,curtHp + "/" + hpMax);
        imgSelfHp.fillAmount = curtHp * 1.0f / hpMax;
    }

    public bool GetCanSkill()
    {
        return BattleMgr.Instance.CanRlsSkill();
    }

    #region Boss Hp Bar

    private float curtPrg = 1f;
    private float targetPrg = 1f;
    
    public void SetBossHpBarState(bool state, float prg = 1)
    {
        SetActive(transBossHpBar,state);
        imgBoosHpRed.fillAmount = prg;
        imgBossHpYellow.fillAmount = prg;
    }

    public void SetBossHpBarVal(int oldVal,int newVal,int sumVal)
    {
        curtPrg = oldVal * 1f / sumVal;
        targetPrg = newVal * 1f / sumVal;
        imgBoosHpRed.fillAmount = targetPrg;
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

    #endregion
   

    #region Click Events

    private void RegClickEvents()
    {
        OnClickDown(imgDirBg.gameObject,(PointerEventData data) =>
        {
            startPos = data.position;
            SetActive(imgDirPoint);
            imgDirBg.transform.position = data.position;
        });
        
        OnClickUp(imgDirBg.gameObject,(PointerEventData data) =>
        {
            imgDirBg.transform.position = defaultPos;
            SetActive(imgDirPoint,false);
            imgDirPoint.transform.localPosition = Vector3.zero;
            // TODO 方向信息传递
            curtDir = Vector2.zero;
            BattleSys.Instance.SetMoveDir(curtDir);
        });
        
        OnDrag(imgDirBg.gameObject,(PointerEventData data) =>
        {
            Vector2 dir = data.position - startPos;
            float len = dir.magnitude;
            if (len > pointDis)
            {
                // 把方向向量限制到maxLength的长度
                Vector2 clampDir = Vector2.ClampMagnitude(dir,pointDis);
                imgDirPoint.transform.position = startPos + clampDir;
            }
            else
            {
                imgDirPoint.transform.position = data.position;
            }
            // TODO 方向信息传递
            curtDir = dir.normalized;
            BattleSys.Instance.SetMoveDir(curtDir);
            
        });
    }
    
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWindowState(false);
    }
    
    public void ClickNormalAtk()
    {
        BattleSys.Instance.ReqReleaseSkill(0);
    }
    
    public void ClickSkill1()
    {
        if (isSK1CD == false && GetCanSkill())
        {
            BattleSys.Instance.ReqReleaseSkill(1);
            isSK1CD = true;
            //SetActive(txtSk1CD);
            SetActive(imgSk1CD);
            imgSk1CD.fillAmount = 1;
            sk1CdCount = 0;
            sk1CdNum = (int) sk1CdTime;
            SetText(txtSk1CD,sk1CdNum);
            
        }
        
    }

    public void ClickSkill2()
    {
        if (isSK2CD == false && GetCanSkill())
        {
            BattleSys.Instance.ReqReleaseSkill(2);
            isSK2CD = true;
            //SetActive(txtSk1CD);
            SetActive(imgSk2CD);
            imgSk2CD.fillAmount = 1;
            sk2CdCount = 0;
            sk2CdNum = (int) sk2CdTime;
            SetText(txtSk2CD,sk2CdNum);
            
        }
    }
    
    public void ClickSkill3()
    {
        if (isSK3CD == false && GetCanSkill())
        {
            BattleSys.Instance.ReqReleaseSkill(3);
            isSK3CD = true;
            //SetActive(txtSk1CD);
            SetActive(imgSk3CD);
            imgSk3CD.fillAmount = 1;
            sk3CdCount = 0;
            sk3CdNum = (int) sk3CdTime;
            SetText(txtSk3CD,sk3CdNum);
            
        }
    }

    #endregion


}