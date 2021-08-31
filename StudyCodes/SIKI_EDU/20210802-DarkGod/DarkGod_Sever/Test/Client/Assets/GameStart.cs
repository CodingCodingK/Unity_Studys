/****************************************************
    文件：GameStart.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：#CreateTime#
    功能：Unknown
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using PENet;
using Protocol;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    private PENet.PESocket<ClientSession, NetMsg> client;
    
    private void Start()
    {
        client = new PESocket<ClientSession, NetMsg>();
        client.StartAsClient(IPCfg.srvIP,IPCfg.srvPort);
        
        // 设置log打印
        client.SetLog(true, (string msg, int lv) =>
        {
            switch (lv)
            {
                case 0:
                    msg = "Log:" + msg;
                    Debug.Log(msg);
                    break;
                case 1:
                    msg = "Warning:" + msg;
                    Debug.LogWarning(msg);
                    break;
                case 2:
                    msg = "Error:" + msg;
                    Debug.LogError(msg);
                    break;
                case 3:
                    msg = "Info:" + msg;
                    Debug.Log(msg);
                    break;
            }
        });
        Debug.Log("Start");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            client.session.SendMsg(new NetMsg
            {
                text = "Hello Unity"
            });
        }
    }
}
