using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseWindow
{
	#region 基本属性

	/// <summary>
	/// 窗体
	/// </summary>
	protected Transform transform;
	/// <summary>
	/// 资源名称
	/// </summary>
	protected string resName;
	/// <summary>
	/// 是否常驻
	/// </summary>
	protected bool resident;
	/// <summary>
	/// 是否可见
	/// </summary>
	protected bool visible = false;
	/// <summary>
	/// 窗体类型
	/// </summary>
	protected WindowsType windowType;
	/// <summary>
	/// 场景类型
	/// </summary>
	protected ScenesType sceneType;

	#endregion

	#region 控件列表

	/// <summary>
	/// UI控件 按钮
	/// </summary>
	protected Button[] buttonList;

	#endregion

	/// <summary>
	/// 初始化
	/// </summary>
	protected virtual void Awake()
	{
		//表示隐藏的物体也会查找
		buttonList = transform.GetComponentsInChildren<Button>(true);
		RegisterUIEvent();
	}

	/// <summary>
	/// UI事件注册
	/// </summary>
	protected virtual void RegisterUIEvent()
	{

	}

	/// <summary>
	/// 添加监听
	/// </summary>
	protected virtual void OnAddListener()
	{

	}

	/// <summary>
	/// 移除监听
	/// </summary>
	protected virtual void OnRemoveListener()
	{

	}

	/// <summary>
	/// 每次启动
	/// </summary>
	protected virtual void OnEnable()
	{

	}

	/// <summary>
	/// 每次禁用
	/// </summary>
	protected virtual void OnDisable()
	{

	}

	/// <summary>
	/// 更新
	/// </summary>
	public virtual void Update(float deltaTime)
	{

	}


    #region For WindowManager

    public void Open()
    {
	    if (transform == null)
	    {
		    if (Create())
		    {
			    Awake();//初始化
		    }
	    }

	    if (transform.gameObject.activeSelf == false)
	    {
		    UIRoot.SetParent(transform, true, windowType == WindowsType.TipsWindow);
		    transform.gameObject.SetActive(true);
		    visible = true;
		    OnEnable();//调用激活时候触发的事件
		    OnAddListener();//添加事件
	    }
    }

    public void Close(bool isDestroy = false)
    {
	    if (transform.gameObject.activeSelf == true)
	    {
		    OnRemoveListener();//移除游戏事件
		    OnDisable();//隐藏时候触发的事件
		    if (isDestroy == false)
		    {
			    if (resident)
			    {
				    transform.gameObject.SetActive(false);
				    UIRoot.SetParent(transform, false, false);

			    }
			    else
			    {
				    GameObject.Destroy(transform.gameObject);
				    transform = null;
			    }
		    }
		    else
		    {
			    GameObject.Destroy(transform.gameObject);
			    transform = null;
		    }


	    }
	    //不可见的状态
	    visible = false;
    }

    public void PreLoad()
    {
	    if (transform == null)
	    {
		    if (Create())
		    {
			    Awake();
		    }
	    }
    }

    //获取场景类型
    public ScenesType GetScenesType()
    {
	    return sceneType;
    }

    //窗体类型
    public WindowsType GetWindowType()
    {
	    return windowType;
    }

    //获取根节点
    public Transform GetRoot()
    {
	    return transform;
    }

    //是否可见
    public bool IsVisible()
    {
	    return visible;
    }

    //是否常驻
    public bool IsREsident()
    {
	    return resident;
    }

    #endregion


    private bool Create()
    {
        if (string.IsNullOrEmpty(resName))
        {
            return false;
        }

        if (transform == null)
        {
            var obj = Resources.Load<GameObject>(resName);
            if (obj == null)
            {
                Debug.LogError($"未找到UI预制件{windowType}");
                return false;
            }
            transform = GameObject.Instantiate(obj).transform;

            transform.gameObject.SetActive(false);

            UIRoot.SetParent(transform, false, windowType == WindowsType.TipsWindow);
        }

        return true;
    }

}

/// <summary>
/// 窗体类型
/// </summary>
public enum WindowsType
{
	LoginWindow,
	StoreWindow,
	TipsWindow,
}

/// <summary>
/// 场景类型：根据提供的场景类型进行预加载
/// </summary>
public enum ScenesType
{
	None,
	Login,
	Battle
}