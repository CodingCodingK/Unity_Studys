/****************************************************
    文件：NetSvc.cs
    作者：YAN
    邮箱：2470939431@qq.com
    日期：2021/8/31 22:57:52
    功能：网络服务
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using PEProtocol;
using PENet;
using UnityEngine;
using LogType = PEProtocol.LogType;

public class NetSvc : GameRootMonoSingleton<NetSvc>
{
    private PESocket<ClientSession, GameMsg> client = null;
    
    /// <summary>
    /// 客户端向服务端发送消息的队列
    /// </summary>
    private Queue<GameMsg> msgQue = null;
    
    public static readonly string obj = "lock";
    
    public void InitSvc()
    {
        client = new PESocket<ClientSession, GameMsg>();
        msgQue = new Queue<GameMsg>();
        
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
        
        // 启动
        client.StartAsClient(ServerConfig.srvIP,ServerConfig.srvPort);

        Debug.Log("NetSvc Init Completed.");
    }

    public void SendMsg(GameMsg msg)
    {
        if (client.session != null)
        {
            client.session.SendMsg(msg);
        }
        else
        {
            GameRootResources.Instance().ShowTips("服务器未连接");
            InitSvc();
        }
    }

    /// <summary>
    /// 向客户端发送请求
    /// </summary>
    public void AddNetMsg(GameMsg msg)
    {
        lock (obj)
        {
            msgQue.Enqueue(msg);
        }
    }

    void Update()
    {
        if (msgQue?.Count > 0)
        {
            lock (obj)
            {
                var msg = msgQue.Dequeue();
                HandleRsp(msg);
            }
        }
    }
    
    /// <summary>
    /// 分发处理 消息
    /// </summary>
    private void HandleRsp(GameMsg msg)
    {
        if (msg.err != (int)ErrorCode.None)
        {
            switch ((ErrorCode)msg.err)
            {
                case ErrorCode.AccountIsOnline :
                    GameRootResources.Instance().ShowTips("当前账号已在线！");
                    break;
                case ErrorCode.WrongPass :
                    GameRootResources.Instance().ShowTips("输入账户名或密码错误！");
                    break;
                case ErrorCode.ServerDataError :
                    PECommon.Log("服务端数据异常",LogType.Error);
                    GameRootResources.Instance().ShowTips("服务端数据异常！");
                    break;
                case ErrorCode.ClientDataError :
                    PECommon.Log("客户端数据异常",LogType.Error);
                    GameRootResources.Instance().ShowTips("客户端数据异常！");
                    break;
                case ErrorCode.UpdateDBError :
                    PECommon.Log("数据库更新异常",LogType.Error);
                    GameRootResources.Instance().ShowTips("网络不稳定！");
                    break;
                case ErrorCode.LackLevel :
                    PECommon.Log("作弊检测：等级不足",LogType.Error);
                    GameRootResources.Instance().ShowTips("等级不足");
                    break;
                case ErrorCode.LackCoin :
                    PECommon.Log("作弊检测：金币不足",LogType.Error);
                    GameRootResources.Instance().ShowTips("金币不足");
                    break;
                case ErrorCode.LackCrystal :
                    PECommon.Log("作弊检测：晶体不足",LogType.Error);
                    GameRootResources.Instance().ShowTips("晶体不足");
                    break;
                case ErrorCode.LackDiamond :
                    PECommon.Log("作弊检测：钻石不足",LogType.Error);
                    GameRootResources.Instance().ShowTips("钻石不足");
                    break;
                case ErrorCode.LackPower :
                    PECommon.Log("作弊检测：体力不足",LogType.Error);
                    GameRootResources.Instance().ShowTips("体力不足");
                    break;
            }
            
            return;
        }
        
        switch ((CMD)msg.cmd)
        {
            case CMD.RspLogin:
                LoginSys.Instance.RspLogin(msg);
                break;
            case CMD.RspRename:
                LoginSys.Instance.RspRename(msg);
                break;
            case CMD.RspGuide:
                MainCitySys.Instance.RspGuide(msg);
                break;
            case CMD.RspStrong:
                MainCitySys.Instance.RspStrong(msg);
                break;
            case CMD.PushChat:
                MainCitySys.Instance.PushChat(msg);
                break;
            case CMD.RspBuy:
                MainCitySys.Instance.RspBuy(msg);
                break;
            case CMD.PushPower:
                MainCitySys.Instance.PushPower(msg);
                // MainCitySys.Instance.PushTaskPrgs(msg); TODO 如果想并包，两次请求操作合成一次，可以这样
                break;
            case CMD.RspTask:
                MainCitySys.Instance.RspTask(msg);
                break;
            case CMD.PushTaskPrgs:
                MainCitySys.Instance.PushTaskPrgs(msg);
                break;
            case CMD.RspDungeon:
                DungeonSys.Instance.RspDungeon(msg);
                break;
        }
    }
    
    
}
