using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 服务器初始化
/// </summary>
public class ServerRoot : Singleton<ServerRoot>
{
    private ServerRoot(){}
    
	private int sessionId = 0;

    public void Init()
    {
        // 数据层
        CacheSvc.Instance().Init();
        DBMgr.Instance().Init();

		// 服务层
		NetSvc.Instance().Init();
		CfgSvc.Instance().Init();
		TimerSvc.Instance().Init();

		// 业务系统层
		LoginSys.Instance().Init();
        GuideSys.Instance().Init();
        StrongSys.Instance().Init();
        ChatSys.Instance().Init();
        BuySys.Instance().Init();
        PowerSys.Instance().Init();

	}

    public void Update()
    {
        NetSvc.Instance().Update();
		TimerSvc.Instance().Update();
    }

	/// <summary>
	/// 分配新的SessionId给客户端
	/// </summary>
	public int GetSessionId()
    {
	    return sessionId == int.MaxValue ? 0 : sessionId++;
    }

}