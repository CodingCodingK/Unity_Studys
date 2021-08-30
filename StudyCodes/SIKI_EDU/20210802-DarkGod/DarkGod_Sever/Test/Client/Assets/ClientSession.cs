/****************************************************
    文件：NewBehaviourScript.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：#CreateTime#
    功能：Unknown
*****************************************************/


using PENet;
using Protocol;

public class ClientSession : PENet.PESession<NetMsg>
{
    protected override void OnConnected()
    {
        PETool.LogMsg("Server Connected !!!");
    }

    protected override void OnDisConnected()
    {
        PETool.LogMsg("Server DisConnect !!!");
    }

    protected override void OnReciveMsg(NetMsg msg)
    {
        PETool.LogMsg("Client Req:" + msg.text);
    }
}
