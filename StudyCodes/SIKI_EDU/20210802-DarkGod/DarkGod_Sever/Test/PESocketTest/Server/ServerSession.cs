using PENet;
using Protocol;

namespace Server
{
    public class ServerSession : PENet.PESession<NetMsg>
    {
        protected override void OnConnected()
        {
            PETool.LogMsg("Client Connected !!!");
        }

        protected override void OnDisConnected()
        {
            PETool.LogMsg("Client DisConnect !!!");
        }

        protected override void OnReciveMsg(NetMsg msg)
        {
            PETool.LogMsg("Client Req:" + msg.text);
        }
    }
}
