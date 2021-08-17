/****************************************************
    文件：ResSvc.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/12 21:50:41
    功能：资源加载服务
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : GameRootMonoSingleton<ResSvc>
{
    // private Action prgCallBack = null;
    
    public void InitSvc()
    {
        Debug.Log("Init ResSvc.");
    }

    public void AsyncLoadScene(string sceneName,Action afterAll)
    {
        
        StartCoroutine(StartLoading(sceneName,afterAll));

        
        // var callback = SceneManager.LoadSceneAsync(sceneName);
        // prgCallBack = () =>
        // {
        //     var progress = callback.progress;
        //     loadingWindow.SetProgress(progress);
        //     if (progress >= 0.9f)
        //     {
        //         loadingWindow.gameObject.SetActive(false);
        //         Debug.Log("SetProgress");
        //         LoginSys.Instance().OpenLoginWindow();
        //         prgCallBack = null;
        //     }
        // };
    }

    private void Update()
    {
        // if (prgCallBack != null)
        // {
        //     prgCallBack();
        // }
    }
    
    /// <summary>
    /// 优化进度读取：协程刷新进度
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator StartLoading(string sceneName,Action afterAll)
    {
        var loadingWindow = GameRootResources.Instance().loadingWindow;
        loadingWindow.SetWindowState(true);
        
        int displayProgress = 0;
        int toProgress = 0;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName); 
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            toProgress = (int)(op.progress * 100);
            // Debug.Log("below90: " + displayProgress + " , " + op.progress + " , " + toProgress);
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                GameRootResources.Instance().loadingWindow.SetProgress(displayProgress);
                yield return new WaitForEndOfFrame();
            }
        }

        toProgress = 100;
       
        while (displayProgress < toProgress)
        {
            // Debug.Log("over90: " + displayProgress + " , " + op.progress);
            ++displayProgress;
            GameRootResources.Instance().loadingWindow.SetProgress(displayProgress);
            yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;
        
        loadingWindow.SetWindowState(false);
        if (afterAll != null)
        {
            afterAll();
        }
    }
    
    
}
