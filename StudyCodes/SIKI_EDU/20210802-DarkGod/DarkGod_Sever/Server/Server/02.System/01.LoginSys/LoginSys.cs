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
        // 当前帐号是否已上线，若未上线账号是否存在，若账号密码是否正确，否则皆返回失败
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

    /// <summary>
    /// 处理 登录请求消息
    /// </summary>
    public void ReqRename(MsgPack msgPack)
    {
        ReqRename data = msgPack.msg.reqRename;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspRename,
        };

        // 名字是否已存在
        if (cacheSvc.IsNameExisted(data.name))
        {
            msg.err = (int)ErrorCode.NameExisted;
        }
        else
        {
            // 不存在重名，就更新缓存+DB，并返回客户端
            PlayerData pd = cacheSvc.GetPlayerDataBySession(msgPack.session);

            // 已登录
            if (pd != null)
            {
                pd.name = data.name;
                cacheSvc.UpdatePlayerData(pd);
                msg.rspRename = new RspRename{ name = pd.name };

            }
            else
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
        }

        msgPack.session.SendMsg(msg);
    }

	/// <summary>
	/// 帐号登出时，清空本地缓存中该账号记录 
	/// </summary>
	public void ClearOfflineData(ServerSession session)
    {
	    cacheSvc.AcctOffline(session);
    }
}