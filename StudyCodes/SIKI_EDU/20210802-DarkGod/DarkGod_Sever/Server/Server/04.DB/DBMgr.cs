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
		//QueryPlayerData("test1","test");
		PECommon.Log("DBMgr Init Done.");
	}

	#region 账号相关

	public PlayerData QueryPlayerData(string acct, string pass)
	{
		PlayerData playerData = null;

		MySqlCommand cmd = new MySqlCommand("select * from account where acct = @acct", conn);
		cmd.Parameters.AddWithValue("acct", acct);


		ReqLogin acctAndPass = cmd.Query<ReqLogin>()?.FirstOrDefault();



		if (acctAndPass != null && pass.Equals(acctAndPass.pass))
		{
			// 帐号密码正确，查询填充具体数据
			playerData = cmd.QueryPlayerData()?.FirstOrDefault();

		}
		else if (acctAndPass != null && !pass.Equals(acctAndPass.pass))
		{
			// 帐号存在、密码不正确           
		}
		else
		{
			// 帐号不存在，新建默认账号数据
			playerData = new PlayerData
			{
				id = -1,
				name = "",
				level = 1,
				exp = 0,
				power = 150,
				coin = 5000,
				diamond = 500,
				crystal = 100,
                hp = 2000,
                ad = 275,
                ap = 265,
                addef = 67,
                apdef = 43,
                dodge = 7,
                pierce = 5,
                critical = 2,
				guideid = 1001,
				strongArr = new int[6]{1,1,1,1,1,1},
				time = TimerSvc.Instance().GetNowTime(),
            };
			playerData.id = InsertNewAcctData(acct, pass, playerData);
		}

		return playerData;
	}

	private int InsertNewAcctData(string acct, string pass, PlayerData pd)
	{
		// TODO add column
		MySqlCommand cmd = new MySqlCommand(
			"insert into account set acct = @acct,pass = @pass,name = @name,level = @level,exp = @exp,power = @power,coin = @coin,diamond = @diamond,crystal = @crystal,hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical,guideid=@guideid,strongArr=@strongArr,time=@time", conn);
		cmd.Parameters.AddWithValue("acct", acct);
		cmd.Parameters.AddWithValue("pass", pass);
		cmd.Parameters.AddWithValue("name", pd.name);
		cmd.Parameters.AddWithValue("level", pd.level);
		cmd.Parameters.AddWithValue("exp", pd.exp);
		cmd.Parameters.AddWithValue("power", pd.power);
		cmd.Parameters.AddWithValue("coin", pd.coin);
		cmd.Parameters.AddWithValue("diamond", pd.diamond);
		cmd.Parameters.AddWithValue("crystal", pd.crystal);
        cmd.Parameters.AddWithValue("hp", pd.hp);
        cmd.Parameters.AddWithValue("ad", pd.ad);
        cmd.Parameters.AddWithValue("ap", pd.ap);
        cmd.Parameters.AddWithValue("addef", pd.addef);
        cmd.Parameters.AddWithValue("apdef", pd.apdef);
        cmd.Parameters.AddWithValue("dodge", pd.dodge);
        cmd.Parameters.AddWithValue("pierce", pd.pierce);
        cmd.Parameters.AddWithValue("critical", pd.critical);
        cmd.Parameters.AddWithValue("guideid", pd.guideid);
        cmd.Parameters.AddWithValue("time", pd.time);

        cmd.Parameters.AddWithValue("strongArr", pd.strongArr.ToStringArr());

        cmd.ExecuteNonQuery();
		return (int)cmd.LastInsertedId;
	}

    public PlayerData QueryPlayerDataByName(string name)
    {
        PlayerData playerData = null;

        MySqlCommand cmd = new MySqlCommand("select * from account where name = @name", conn);
        cmd.Parameters.AddWithValue("name",name);

        return cmd.QueryPlayerData()?.FirstOrDefault();
	}

    public void UpdatePlayerData(PlayerData pd)
    {
		MySqlCommand cmd = new MySqlCommand(
			"update account set name = @name,level = @level,exp = @exp,power = @power,coin = @coin,diamond = @diamond,crystal = @crystal,hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical,guideid=@guideid,strongArr=@strongArr,time=@time where id = @id", conn);
        cmd.SetPlayerDataParas(pd);
        cmd.ExecuteNonQuery();
    }

	#endregion


}

