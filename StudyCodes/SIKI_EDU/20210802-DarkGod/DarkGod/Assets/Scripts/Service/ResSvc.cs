/****************************************************
    文件：ResSvc.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/12 21:50:41
    功能：资源加载服务
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : GameRootMonoSingleton<ResSvc>
{
    public void Start()
    {
        Debug.Log("Init ResSvc Start.");
    }
    public void InitSvc()
    {
        Debug.Log("Init ResSvc.");
    }

    public void AsyncLoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
