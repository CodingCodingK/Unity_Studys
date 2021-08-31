/****************************************************
    文件：AudioSvc.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/18 22:16:2
    功能：音频加载服务
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSvc : GameRootMonoSingleton<AudioSvc>
{
    public AudioSource bgAudio;
    public AudioSource uiAudio;

    public void InitSvc()
    {
        Debug.Log("AudioSvc Init Completed.");
    }

    public void PlayBGMusic(string name, bool isLoop = true)
    {
        AudioClip audioClip = ResSvc.Instance().LoadAudio("ResAudio/" + name);
        // 检测是否和播放的一样
        if (bgAudio.clip == null || bgAudio.clip.name != audioClip.name)
        {
            bgAudio.clip = audioClip;
            bgAudio.loop = true;
        }

        if (!bgAudio.isPlaying)
        {
            bgAudio.Play();
        }
        
    }
    
    public void PlayUIAudio(string name, bool isLoop = true)
    {
        AudioClip uiClip = ResSvc.Instance().LoadAudio("ResAudio/" + name);
        uiAudio.clip = uiClip;
        uiAudio.Play();
    }
    
}
