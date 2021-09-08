using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocol;

/// <summary>
/// 带有Session、GameMsg的包装类，使用Session类发送GameMsg
/// </summary>
public class MsgPack
{
    public ServerSession session;
    public GameMsg msg;

    public MsgPack(ServerSession s, GameMsg m)
    {
        session = s;
        msg = m;
    }
}