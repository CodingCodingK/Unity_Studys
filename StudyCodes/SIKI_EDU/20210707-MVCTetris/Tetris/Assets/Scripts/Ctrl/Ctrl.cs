using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl : MonoBehaviour
{
    [HideInInspector]
    public Model model;
    [HideInInspector]
    public View view;
    [HideInInspector]
    public CameraManager cameraManager;
    [HideInInspector]
    public GameManager gameManager;
    
    private FSMSystem fsm;
    // Start is called before the first frame update
    void Awake()
    {
        model = GameObject.FindGameObjectWithTag("Model").GetComponent<Model>();
        view = GameObject.FindGameObjectWithTag("View").GetComponent<View>();
        cameraManager = GetComponent<CameraManager>();
        gameManager = GetComponent<GameManager>();
    }

    private void Start()
    {
        MakeFSM();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeFSM()
    {
        fsm = new FSMSystem();
        var states = GetComponentsInChildren<FSMState>();
        foreach (var fsmState in states)
        {
            fsm.AddState(fsmState,this);
        }

        // 设置初始状态
        var menuState = GetComponentInChildren<MenuState>();
        fsm.SetCurrentState(menuState);
    }
}
