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
