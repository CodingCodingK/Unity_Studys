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
using System.Xml;
using PEProtocol;
using UnityEngine;
using UnityEngine.SceneManagement;
using LogType = PEProtocol.LogType;

public class ResSvc : GameRootMonoSingleton<ResSvc>
{
    // private Action prgCallBack = null;

    /// <summary>
    /// Audio暂存池
    /// </summary>
    private Dictionary<string, AudioClip> audioDic = new Dictionary<string, AudioClip>();
    
    public void InitSvc()
    {
        Debug.Log("ResSvc Init Completed.");
        InitRDNameConfig();
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

    /// <summary>
    /// 加载Audio
    /// </summary>
    /// <param name="path"></param>
    /// <param name="isCache">是否需要放进缓存字典中</param>
    /// <returns></returns>
    public AudioClip LoadAudio(string path, bool isCache = true)
    {
        AudioClip au = null;
        if (!audioDic.TryGetValue(path,out au))
        {
            au = Resources.Load<AudioClip>(path);
            if (isCache)
            {
                audioDic[path] = au;
            }
        }
        
        return au;
    }

    #region Configs

    private List<string> surnameList = new List<string>();
    private List<string> manList = new List<string>();
    private List<string> womanList = new List<string>();

    /// <summary>
    /// 读取随机名字配置文件
    /// </summary>
    private void InitRDNameConfig()
    {
        TextAsset xml = Resources.Load<TextAsset>(PathDefine.RDNameConfig);
        if (!xml)
        {
            PECommon.Log("xml file:"+PathDefine.RDNameConfig + "not exist",LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodList.Count; i++)
            {
                XmlElement ele = nodList[i] as XmlElement;
                var eleID = ele.GetAttributeNode("ID");
                if (eleID == null)
                {
                    continue;
                }

                int ID = Convert.ToInt32(eleID.InnerText);
                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "surname":
                            surnameList.Add(e.InnerText);
                            break;
                        case "man":
                            manList.Add(e.InnerText);
                            break;
                        case "woman":
                            womanList.Add(e.InnerText);
                            break;
                    }
                }
            }
        }
    }

    public string GetRDName(bool man = true)
    {
        var surname = surnameList[PETools.RDInt(0, surnameList.Count - 1)];
        var givenName = man
            ? manList[PETools.RDInt(0, manList.Count - 1)]
            : womanList[PETools.RDInt(0, womanList.Count - 1)];
        return surname + givenName;
    }

    #endregion
}
