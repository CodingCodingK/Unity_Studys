using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
using UnityEngine;

public class Shape : MonoBehaviour
{
    private Ctrl ctrl;
    
    private bool isPause = false;

    private float timer = 0;

    private float stepTime = 0.8f;

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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPause) return;
        {
            timer += Time.deltaTime;
            if (timer >= stepTime)
            {
                timer = 0;
                Fall();
            }
        }
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
            ctrl.model.PlaceShape(transform);
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
