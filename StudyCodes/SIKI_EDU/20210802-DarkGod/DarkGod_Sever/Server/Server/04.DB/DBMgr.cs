using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBHelper;
using MySql.Data.MySqlClient;
using PEProtocol;

/// <summary>
/// 数据库管理类
/// </summary>
public class DBMgr : Singleton<DBMgr>
{
	private DBMgr() { }

	/// <summary>
	/// 连接服务
	/// </summary>
	private MySqlConnection conn;

	public void Init()
	{
		conn = new MySqlConnection("server=localhost;User Id = root;password=;Database=darkgod;Charset = utf8");
		conn.Open();
		QueryPlayerData("test1","test");
		PECommon.Log("DBMgr Init Done.");
	}

	public PlayerData QueryPlayerData(string acct, string pass)
	{
		PlayerData playerData = null;

		MySqlCommand cmd = new MySqlCommand("select * from account where acct = @acct", conn);
		cmd.Parameters.AddWithValue("acct", acct);

		playerData = cmd.Query<PlayerData>()?.FirstOrDefault();

		
		if (playerData != null && pass.Equals(playerData.pass))
		{
			// 帐号密码正确

		}
		else if (playerData != null && !pass.Equals(playerData.pass))
		{
			// 帐号存在、密码不正确
			playerData = null;
		}
		else
		{
			// 帐号不存在，新建默认账号数据
			playerData = new PlayerData
			{
				id = -1,
				acct = acct,
				pass = pass,
				name = "",
				level = 1,
				exp = 0,
				power = 150,
				coin = 5000,
				diamond = 500,
			};
			playerData.id = InsertNewAcctData(playerData);
		}

		return playerData;
	}

	private int InsertNewAcctData(PlayerData pd)
	{
		MySqlCommand cmd = new MySqlCommand(
			"insert into account set acct = @acct,pass = @pass,name = @name,level = @level,exp = @exp,power = @power,coin = @coin,diamond = @diamond", conn);
		cmd.Parameters.AddWithValue("acct", pd.acct);
		cmd.Parameters.AddWithValue("pass", pd.pass);
		cmd.Parameters.AddWithValue("name", pd.name);
		cmd.Parameters.AddWithValue("level", pd.level);
		cmd.Parameters.AddWithValue("exp", pd.exp);
		cmd.Parameters.AddWithValue("power", pd.power);
		cmd.Parameters.AddWithValue("coin", pd.coin);
		cmd.Parameters.AddWithValue("diamond", pd.diamond);

		cmd.ExecuteNonQuery();
		return (int)cmd.LastInsertedId;
	}

}

