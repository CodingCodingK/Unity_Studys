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

    private CacheSvc cacheSvc;
    public void Init()
    {
        cacheSvc = CacheSvc.Instance();
        PECommon.Log("LoginSys Init Done.");
    }

    /// <summary>
    /// 处理 登录请求消息
    /// </summary>
    /// <param name="msg"></param>
    public void ReqLogin(MsgPack msgPack)
    {
        // TODO 当前帐号是否已上线，若未上线账号是否存在，若账号密码是否正确，否则皆返回失败
        ReqLogin data = msgPack.msg.reqLogin;

        GameMsg msg = new GameMsg
        {
            cmd = (int) CMD.RspLogin,
        };

        // 当前帐号是否已上线
        if (cacheSvc.IsAccOnline(data.acct))
        {
            msg.err = (int)ErrorCode.AccountIsOnline;
        }
        else
        {
            PlayerData pd = cacheSvc.GetPlayerData(data.acct,data.pass);

            if (pd == null)
            {
                // 为空，密码错误
                msg.err = (int)ErrorCode.WrongPass;
            }
            else
            {
                // 登录认证成功
                msg.rspLogin = new RspLogin
                {
                    playerData = pd
                };

                cacheSvc.AcctOnline(data.acct, msgPack.session, pd);
            }
        }

        msgPack.session.SendMsg(msg);
    }
}