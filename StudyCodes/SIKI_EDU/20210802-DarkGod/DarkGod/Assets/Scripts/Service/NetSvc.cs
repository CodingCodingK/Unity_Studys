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

public class NetSvc : GameRootMonoSingleton<NetSvc>
{
    private PESocket<ClientSession, GameMsg> client = null;
    public void InitSvc()
    {
        client = new PESocket<ClientSession, GameMsg>();
        
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
}
