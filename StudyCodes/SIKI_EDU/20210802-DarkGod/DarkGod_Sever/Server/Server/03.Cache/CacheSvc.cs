using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocol;

/// <summary>
/// 缓存层
/// </summary>
public class CacheSvc : Singleton<CacheSvc>
{
    protected CacheSvc(){}

    /// <summary>
    /// 账号在线字典
    /// </summary>
    private Dictionary<string, ServerSession> onlineAcctDic;

	/// <summary>
	/// 在线账号信息字典
	/// </summary>
	private Dictionary<ServerSession, PlayerData> onlineSessionDic;

    public void Init()
    {
        onlineAcctDic = new Dictionary<string, ServerSession>();
        onlineSessionDic = new Dictionary<ServerSession, PlayerData>();
        PECommon.Log("CacheSvc Init Done.");
    }

    /// <summary>
    /// 检测账号是否在线
    /// </summary>
    public bool IsAccOnline(string acct)
    {
        return onlineAcctDic.ContainsKey(acct);
    }

    /// <summary>
    /// 根据账号密码返回 PlayerData 账号数据，否则返回null
    /// </summary>
    public PlayerData GetPlayerData(string acct, string pass)
    {
	    return DBMgr.Instance().QueryPlayerData(acct, pass);
    }

    /// <summary>
    /// 帐号上线，缓存数据
    /// </summary>
    public void AcctOnline(string acct,ServerSession session,PlayerData playerData)
    {
        onlineAcctDic.Add(acct, session);
        onlineSessionDic.Add(session, playerData);
    }
}
