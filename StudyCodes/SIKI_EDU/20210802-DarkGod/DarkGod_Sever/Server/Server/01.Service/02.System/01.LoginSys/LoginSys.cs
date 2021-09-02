using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PENet;
using PEProtocol;

/// <summary>
/// 登陆业务系统
/// </summary>
public class LoginSys : Singleton<LoginSys>
{
    private LoginSys() { }

    public void Init()
    {
        PECommon.Log("LoginSys Init Done.");
    }
}