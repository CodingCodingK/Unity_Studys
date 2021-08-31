/****************************************************
    文件：NewBehaviourScript.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：#CreateTime#
    功能：Unknown
*****************************************************/


using PENet;
using Protocol;
using UnityEngine;

public class ClientSession : PENet.PESession<NetMsg>
{
    protected override void OnConnected()
    {
        Debug.Log("Server Connected !!!");
    }

    protected override void OnDisConnected()
    {
        Debug.Log("Server DisConnect !!!");
    }

    protected override void OnReciveMsg(NetMsg msg)
    {
        Debug.Log("Client Req:" + msg.text);
    }
}
