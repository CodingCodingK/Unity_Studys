using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadGame : MonoBehaviour {

    public Slider processView;

	// Use this for initialization
	void Start () {
        LoadGameMethod();
        
	}
	
	// Update is called once per frame
	void Update () {
		

    }
    public void LoadGameMethod()
    {
        StartCoroutine(LoadResourceCoroutine());
        StartCoroutine(StartLoading_4(2));
    }

    private IEnumerator StartLoading_4(int scene)
    {
        int displayProgress = 0;
        int toProgress = 0;
        AsyncOperation op = SceneManager.LoadSceneAsync(scene); 
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            toProgress = (int)(op.progress * 100);
            Debug.Log("below90: " + displayProgress + " , " + op.progress + " , " + toProgress);
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                SetLoadingPercentage(displayProgress);
                yield return new WaitForEndOfFrame();
            }
        }

        toProgress = 100;
       
        while (displayProgress < toProgress)
        {
            Debug.Log("over90: " + displayProgress + " , " + op.progress);
            ++displayProgress;
            SetLoadingPercentage(displayProgress);
            yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;
    }

    IEnumerator LoadResourceCoroutine()
    {
        UnityWebRequest request1 = UnityWebRequest.Get(@"http://localhost/fish.lua.txt");
        yield return request1.SendWebRequest();
        var str1 = request1.downloadHandler.text;
        File.WriteAllText(@"D:\github\Unity\StudyCodes\SIKI_EDU\20210627-XLua_JoyFish\XLua_FishingJoy\Assets\PlayerGamePackage\fish.lua.txt",str1);
        UnityWebRequest request2 = UnityWebRequest.Get(@"http://localhost/fishDispose.lua.txt");
        yield return request2.SendWebRequest();
        var str2 = request2.downloadHandler.text;
        File.WriteAllText(@"D:\github\Unity\StudyCodes\SIKI_EDU\20210627-XLua_JoyFish\XLua_FishingJoy\Assets\PlayerGamePackage\fishDispose.lua.txt",str2);
    }

    private void SetLoadingPercentage(float v)
    {
        processView.value = v / 100;
    }

   
}
