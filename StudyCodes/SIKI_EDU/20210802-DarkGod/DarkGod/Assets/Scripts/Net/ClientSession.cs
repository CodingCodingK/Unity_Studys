/****************************************************
    文件：ClientSession.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/31 23:4:43
    功能：客户端网络会话
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using PENet;
using UnityEngine;
using PEProtocol;

public class ClientSession : PESession<GameMsg>
{

    protected override void OnConnected()
    {
        Debug.Log("Server Connect");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        Debug.Log("Server Rsp:" + msg.text);
    }

    protected override void OnDisConnected()
    {
        Debug.Log("Server DisConnect");
    }
}
