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
using System.Linq;
using System.Xml;
using PEProtocol;
using UnityEngine;
using UnityEngine.SceneManagement;
using LogType = PEProtocol.LogType;
using MyHelper;

public class ResSvc : GameRootMonoSingleton<ResSvc>
{
    // private Action prgCallBack = null;

    /// <summary>
    /// Audio暂存池
    /// </summary>
    private Dictionary<string, AudioClip> audioDic = new Dictionary<string, AudioClip>();
    
    /// <summary>
    /// Prefab暂存池
    /// </summary>
    private Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
    
    /// <summary>
    /// Sprite暂存池
    /// </summary>
    private Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();
    
    
    public void InitSvc()
    {
        Debug.Log("ResSvc Init Completed.");
        InitRDNameConfig(PathDefine.RDNameConfig);
        InitMapCfg(PathDefine.MapConfig);
        InitAutoGuideCfg(PathDefine.AutoGuideConfig);

    }
    
    private Action sceneBPMethod = null;
    public void AsyncLoadScene(string sceneName,Action afterAll)
    {
        StartCoroutine(StartLoading(sceneName,afterAll));
        
        // GameRootResources.Instance().loadingWindow.SetWindowState();
        //
        // AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);
        // //sceneAsync.allowSceneActivation = true;
        // prgCB = () => {
        //     float val = sceneAsync.progress;
        //     GameRootResources.Instance().loadingWindow.SetProgress(val);
        //     if (val >= 0.9) {
        //         if (afterAll != null) {
        //             afterAll();
        //         }
        //         prgCB = null;
        //         sceneAsync = null;
        //         GameRootResources.Instance().loadingWindow.SetWindowState(false);
        //     }
        // };
    }

    private void Update()
    {
        if (sceneBPMethod != null)
        {
            sceneBPMethod();
            sceneBPMethod = null;
        }
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
        // 卸载当前场景
        
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName); 
        
        // 不让场景自动跳转，progress也最多只能到90%
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
        
        // 赋值回调函数
        sceneBPMethod = afterAll;
        
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

    public GameObject LoadPrefab(string path, bool isCache = false)
    {
        GameObject prefab = null;
        if (!prefabDic.TryGetValue(path,out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if (isCache)
            {
                prefabDic[path] = prefab;
            }
        }
        
        // 再实例化一下
        // GameObject go = null;
        // if (prefab != null)
        // {
        //     go = Instantiate(prefab);
        // }

        return Instantiate(prefab);
    }
    
    public GameObject GetPrefab(string path)
    {
        GameObject prefab = null;
        prefabDic.TryGetValue(path, out prefab);
        return prefab;
    }

    public Sprite LoadSprite(string path,bool cache = false)
    {
        Sprite sp = null;
        if (!spriteDic.TryGetValue(path,out sp))
        {
            sp = Resources.Load<Sprite>(path);
            if (cache)
            {
                spriteDic.Add(path,sp);
            }
        }

        return sp;
    }
    

    #region Configs

    #region 随机名字

    private List<string> surnameList = new List<string>();
    private List<string> manList = new List<string>();
    private List<string> womanList = new List<string>();

    /// <summary>
    /// 读取随机名字配置文件
    /// </summary>
    private void InitRDNameConfig(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:"+ path + "not exist",LogType.Error);
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

    #region 配置数据

    private Dictionary<int, MapCfg> mapCfgDataDic = new Dictionary<int, MapCfg>();
    
    private Dictionary<int, AutoGuideCfg> autoGuideCfgDataDic = new Dictionary<int, AutoGuideCfg>();
    
    public void InitMapCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist",LogType.Error);
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

                int id = Convert.ToInt32(eleID.InnerText);
                MapCfg dto = new MapCfg
                {
                    ID = id,
                };

                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "mapName":
                            dto.mapName = e.InnerText;
                            break;
                        case "sceneName":
                            dto.sceneName = e.InnerText;
                            break;
                        case "mainCamPos":
                            dto.mainCamPos = MapperHelper.ConvertToVector3(e.InnerText);
                            break;
                        case "mainCamRote":
                            dto.mainCamRote = MapperHelper.ConvertToVector3(e.InnerText);
                            break;
                        case "playerBornPos":
                            dto.playerBornPos = MapperHelper.ConvertToVector3(e.InnerText);
                            break;
                        case "playerBornRote":
                            dto.playerBornRote = MapperHelper.ConvertToVector3(e.InnerText);
                            break;
                    }
                }

                mapCfgDataDic.Add(id,dto);
            }
        }
    }
    
    public MapCfg GetMapCfgData(int id)
    {
        return mapCfgDataDic[id];
    }

    public void InitAutoGuideCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml file:" + path + "not exist",LogType.Error);
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

                int id = Convert.ToInt32(eleID.InnerText);
                AutoGuideCfg dto = new AutoGuideCfg
                {
                    ID = id,
                };

                foreach (XmlElement e in nodList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "npcID":
                            dto.npcID = Convert.ToInt32(e.InnerText);
                            break;
                        case "dilogArr":
                            dto.dilogArr = e.InnerText;
                            break;
                        case "actID":
                            dto.actID = Convert.ToInt32(e.InnerText);
                            break;
                        case "coin":
                            dto.coin = Convert.ToInt32(e.InnerText);
                            break;
                        case "exp":
                            dto.exp = Convert.ToInt32(e.InnerText);
                            break;
                       
                    }
                }

                autoGuideCfgDataDic.Add(id,dto);
            }
        }
    }
    
    public AutoGuideCfg GetAutoGuideData(int id)
    {
        return autoGuideCfgDataDic[id];
    }
    
    #endregion
    
    

    #endregion
}
