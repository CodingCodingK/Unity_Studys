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
        GameRootResources.Instance().ShowTips("服务器连接成功");
        Debug.Log("Connect To Server");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        Debug.Log("RcvPack CMD:" + ((CMD)msg.cmd).ToString());
        NetSvc.Instance().AddNetMsg(msg);
    }

    protected override void OnDisConnected()
    {
        GameRootResources.Instance().ShowTips("服务器已断开连接");
        Debug.Log("DisConnect To Server");
    }
}
