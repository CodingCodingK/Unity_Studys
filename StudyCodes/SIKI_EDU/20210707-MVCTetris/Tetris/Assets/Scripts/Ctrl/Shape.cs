using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Newtonsoft.Json.Bson;
using UnityEngine;

public class Shape : MonoBehaviour
{
    private Transform pivot;
    
    private Ctrl ctrl;
    
    private bool isPause = false;

    private float timer = 0;

    private float stepTime = 0.8f;

    /// <summary>
    /// 下落加速倍数
    /// </summary>
    private const float multiple = 16f;

    /// <summary>
    /// 是否加速
    /// </summary>
    private bool isSpeedUp = false;

    private GameManager gameManager;
    
    public void InitData(Color color,Ctrl ctrl,GameManager gameManager)
    {
        this.ctrl = ctrl;
        this.gameManager = gameManager;
        foreach (Transform t in transform)
        {
            // if (t.tag == "Block")
            // {
            //     t.GetComponent<SpriteRenderer>().color = color;
            // }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        pivot = transform.Find("Pivot");
    }

    // Update is called once per frame
    void Update()
    {
        if (isPause) return;
        timer += Time.deltaTime;
        if (timer >= stepTime)
        {
            timer = 0;
            Fall();
        }
        InputControl();
    }

    void Fall()
    {
        var pos = transform.position;
        pos.y -= 1;
        transform.position = pos;
        
        if (ctrl.model.IsValidMapPosition(transform) == false)
        {
            pos.y += 1;
            transform.position = pos;
            isPause = true;
            gameManager.CurrentShapeComplete();
            var isLineClear = ctrl.model.PlaceShape(transform);
            if (isLineClear)
            {
                ctrl.audioManager.PlayAudio("lineClear");
                ctrl.UpdateGameUI();
            }
            return;
        }
        ctrl.audioManager.PlayAudio("drop");
    }

    private void InputControl()
    {
        float h = 0;
        h = Input.GetKeyDown(KeyCode.RightArrow)? 1 : Input.GetKeyDown(KeyCode.LeftArrow) ? -1 : 0;

        if (h!=0)
        {
            Vector3 pos = transform.position;
            pos.x += h;
            transform.position = pos;
            transform.position = pos;
        
            if (ctrl.model.IsValidMapPosition(transform) == false)
            {
                pos.x -= h;
                transform.position = pos;
            }
            else
            {
                ctrl.audioManager.PlayAudio("move");
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(pivot.position,Vector3.forward, -90f);
            if (ctrl.model.IsValidMapPosition(transform) == false)
            {
                transform.RotateAround(pivot.position,Vector3.forward, 90f);
            }
            else
            {
                ctrl.audioManager.PlayAudio("move");
            }
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if(!isSpeedUp)
            {
                isSpeedUp = true;
                stepTime /= multiple;
            }
        }
        else if(isSpeedUp)
        {
            isSpeedUp = false;
            stepTime *= multiple;
        }
    }
    
    public void Pause()
    {
        isPause = true;
    }
    
    public void Resume()
    {
        isPause = false;
    }
}
