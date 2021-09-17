/****************************************************
    文件：Contents.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/12 22:43:0
    功能：常数定数
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    #region 场景名称

    public const string SceneLogin = "SceneLogin";
    
    public const string SceneMainCity = "SceneMainCity";
    
    public const int MapID_MainCity = 10000;

    #endregion

    #region 音效名称

    // 登录bgm
    public const string BGLogin = "bgLogin";
    
    // 主城bgm
    public const string BGMainCity = "bgMainCity";
    
    // 登录按钮音效
    public const string UILoginBtn = "uiLoginBtn";
    
    // 常规UI点击音效
    public const string UIClickBtn = "uiClickBtn";
    public const string UIExtenBtn = "uiExtenBtn";
    public const string UIOpenPage = "uiOpenPage";
    
    #endregion

    #region 屏幕标准宽高

    public const int ScreenStandardWidth = 1334;
    public const int ScreenStandardHeight = 750;

    /// 摇杆点标准操作最大距离
    public const int ScreenOPDis = 70;

    #endregion

    #region 角色属性类

    // 移动速度相关
    public const int PlayerMoveSpeed = 8;
    public const int MonsterMoveSpeed = 4;
    /// 运动平滑 最大加速度(单位秒)
    public const float AccelerSpeed = 4;
    // 混合参数
    public const int BlendIdle = 0;
    public const int BlendWalk = 1;

    #endregion
}
