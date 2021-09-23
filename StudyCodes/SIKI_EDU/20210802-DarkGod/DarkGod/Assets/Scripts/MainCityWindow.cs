/****************************************************
    文件：MainCityWIndow.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/9/14 21:38:54
    功能：主城窗口
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Data;
using Common;
using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainCityWindow : WindowBase
{
    #region UI define

    public Animation menuAni;
    public Button btnMenu;
    private bool menuState = false;
    
    
    // 拖拽范围
    public Image imgTouch;
    // 拖拽背景盘
    public Image imgDirBg;
    // 拖拽点
    public Image imgDirPoint;

    public Text txtFight;
    public Text txtPower;
    public Image imgPowerPrg;
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;
    public Button btnGuide;
    
    /// <summary>
    /// 经验进度条
    /// </summary>
    public Transform expPrgTrans;

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
    
    private AutoGuideCfg curtTaskData;
    
    
    #endregion

    #region Main Func

    protected override void InitWindow()
    {
        base.InitWindow();

        defaultPos = imgDirBg.transform.position;
        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
        
        RegisterTouchEvents();
        SetActive(imgDirPoint,false);
        RefreshUI();
    }

    /// <summary>
    /// 根据数据库取得的 PlayerData 更新 UI
    /// </summary>
    public void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance().PlayerData;
        SetText(txtFight,PECommon.GetFight(pd));
        SetText(txtPower,$"{pd.power}/{PECommon.GetPowerLimit(pd.level)}");
        imgPowerPrg.fillAmount = pd.power * 1.0f / PECommon.GetPowerLimit(pd.level);
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
        
        // 设置自动任务图标
        curtTaskData = resSvc.GetAutoGuideData(pd.guideid);
        if (curtTaskData != null)
        {
            SetGuideBtnIcon(curtTaskData.npcID);
        }
        else
        {
            SetGuideBtnIcon(-1);
        }

    }

    private void SetGuideBtnIcon(int npcID)
    {
        string spPath = "";
        Image img = btnGuide.GetComponent<Image>();
        
        switch (npcID)
        {
            case Constants.NPCWiseMan:
                spPath = PathDefine.WiseManHead;
                break;
            case Constants.NPCGeneral:
                spPath = PathDefine.GeneralHead;
                break;
            case Constants.NPCArtisan:
                spPath = PathDefine.ArtisanHead;
                break;
            case Constants.NPCTrader:
                spPath = PathDefine.TraderHead;
                break;
            default:
                spPath = PathDefine.TaskHead;
                break;
        }

        SetSprite(img, spPath);
    }

    #endregion

    #region Click Events
    
    /// <summary>
    /// 右下角扩展栏 按钮
    /// </summary>
    public void ClickMenuButton()
    {
        audioSvc.PlayUIAudio(Constants.UIExtenBtn);
        menuState = !menuState;
        AnimationClip clip;
        if (menuState)
        {
            clip = menuAni.GetClip("OpenMCMenu");
        }
        else
        {
            clip = menuAni.GetClip("CloseMCMenu");
        }

        menuAni.Play(clip.name);
    }

    /// <summary>
    /// 头像详细信息显示 按钮
    /// </summary>
    public void ClickHeadBtn()
    {
        MainCitySys.Instance.OpenInfoWindow();
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        
    }

    /// <summary>
    /// 托盘事件
    /// </summary>
    public void RegisterTouchEvents()
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
            MainCitySys.Instance.SetMoveDir(Vector3.zero);
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
            MainCitySys.Instance.SetMoveDir(dir.normalized);
        });
    }

    /// <summary>
    /// 自动引导 按钮
    /// </summary>
    public void ClickAutoGuideBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);

        if (curtTaskData != null)
        {
            MainCitySys.Instance.RunTask(curtTaskData);
        }
        else
        {
            GameRootResources.Instance().ShowTips("更多引导任务，正在开发中...");
        }
        
    }
    
    /// <summary>
    /// 头像详细信息显示 按钮
    /// </summary>
    public void ClickStrongBtn()
    {
        MainCitySys.Instance.OpenStrongWindow();
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        
    }
    
    #endregion
    
    
}
