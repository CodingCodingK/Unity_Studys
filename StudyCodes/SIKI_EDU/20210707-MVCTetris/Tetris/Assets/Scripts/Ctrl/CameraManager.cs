using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{

    private Camera mainCamera;
    
    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 显示放大的版本
    /// </summary>
    public void ZoomIn()
    {
        mainCamera.DOOrthoSize(15f, 0.8f);
    }

    /// <summary>
    /// 显示缩小的版本
    /// </summary>
    public void ZoomOut()
    {
        mainCamera.DOOrthoSize(20f, 0.8f);
    }
}
