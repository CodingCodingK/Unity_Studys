using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip cursor;
    public AudioClip drop;
    public AudioClip move;
    public AudioClip lineClear;
    
    private AudioSource audioSource;
    private bool isMute = false;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(string audioName)
    {
        if (isMute) return;
        
        AudioClip clip = null;
        FieldInfo info = GetType().GetField(audioName, BindingFlags.Instance | BindingFlags.Public);
        if (null != info)
        {
            clip = info.GetValue(this) as AudioClip;
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError(clip);
        }
    }

    private void PlayClip(AudioClip clip)
    {
        //if (isMute) return;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
