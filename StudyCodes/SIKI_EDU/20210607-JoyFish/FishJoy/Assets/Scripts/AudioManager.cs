using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            //if (_instance == null) _instance = GameObject.Find("ScriptsHolder").GetComponent<AudioManager>();
            return _instance;
        }
    }

    public AudioSource bgmAudioSource;
    public AudioClip seaWaveClip;
    public AudioClip goldClip;
    public AudioClip rewardClip;
    public AudioClip fireClip;
    public AudioClip changeClip;
    public AudioClip lvUpClip;

    /// <summary>
    /// 是否静音
    /// </summary>
    private bool isMute;

    public bool IsMute
    {
        get => isMute;
    }

    public void PlayEffectSound(AudioClip clip)
    {
        if (!isMute)
        {
            AudioSource.PlayClipAtPoint(clip, Vector3.zero);
            //bgmAudioSource.PlayOneShot(clip);
        }
      
    }

    public void SwitchMuteState(bool isOn)
    {
        isMute = !isOn;
        DoMute();
    }
    void DoMute()
    {
        if (isMute)
            //bgmAudioSource.Pause();
            bgmAudioSource.mute = true;
        else
            //bgmAudioSource.Play(); 
            bgmAudioSource.mute = false;
    }

    // 切换场景的时候，初始化阶段进行检测
    void Awake()
    {
        _instance = this;
        isMute = PlayerPrefs.GetInt("isMute", 0) == 1;
        DoMute();
    }

}
