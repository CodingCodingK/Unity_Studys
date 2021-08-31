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
    
    public void Init()
    {
        //TODO 数据层

        // 服务层
        NetSvc.Instance().Init();

        // 业务系统层
        LoginSys.Instance().Init();
    }

}