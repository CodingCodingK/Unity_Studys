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

    #region Login、PlayerData相关

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
    /// 获取所有在线帐号的Session
    /// </summary>
    public List<ServerSession> GetAllOnlineClients()
    {
	    return onlineAcctDic.Values.ToList();
    }

    /// <summary>
    /// 获取所有在线帐号的Session、PlayerData
    /// </summary>
    public Dictionary<ServerSession, PlayerData> GetAllOnlineClientsAndPD()
    {
	    return onlineSessionDic;
    }


	/// <summary>
	/// 根据账号密码返回 PlayerData 账号数据，密码错误返回null，账号不存在则创建默认账号
	/// </summary>
	public PlayerData GetPlayerData(string acct, string pass)
    {
        return DBMgr.Instance().QueryPlayerData(acct, pass);
    }

    /// <summary>
    /// 根据 Session 获取已登陆的 PlayerData 账号数据
    /// </summary>
    public PlayerData GetPlayerDataBySession(ServerSession session)
    {
        return onlineSessionDic.TryGetValue(session, out PlayerData pd) ? pd : null;
    }

	/// <summary>
	/// 根据 PlayerData.ID 获取已登陆的 Session 数据
	/// </summary>
	public ServerSession GetSessionByPlayerID(int id)
    {
	    foreach (var session in onlineSessionDic)
	    {
		    if (session.Value.id == id)
		    {
			    return session.Key;
		    }
	    }

	    return null;
    }

	/// <summary>
	/// 帐号上线，缓存数据
	/// </summary>
	public void AcctOnline(string acct, ServerSession session, PlayerData playerData)
    {
        onlineAcctDic.Add(acct, session);
        onlineSessionDic.Add(session, playerData);
    }

    /// <summary>
    /// 判断用户名是否存在
    /// </summary>
    /// <returns></returns>
    public bool IsNameExisted(string name)
    {
        return DBMgr.Instance().QueryPlayerDataByName(name) != null;
    }

    public bool UpdatePlayerData(PlayerData player)
    {
        try
        {
            DBMgr.Instance().UpdatePlayerData(player);
            return true;
        }
        catch
        {
            return false;
        }
    }

	/// <summary>
	/// 帐号登出时，清空本地缓存中该账号记录 
	/// </summary>
	internal void AcctOffline(ServerSession session)
    {
	    foreach (var item in onlineAcctDic)
	    {
		    if (item.Value == session)
		    {
			    onlineAcctDic.Remove(item.Key);
			    break;
		    }
	    }

	    string succ = onlineSessionDic.Remove(session) ? "successed" : "failed";
	    PECommon.Log("Offline Result SessionId:"+ session.sessionId + " " + succ);
    }

    #endregion





}
