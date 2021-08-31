using PENet;
using Protocol;

namespace Server
{
    public class ServerSession : PENet.PESession<NetMsg>
    {
        protected override void OnConnected()
        {
            PETool.LogMsg("Client Connected !");
            SendMsg(new NetMsg
            {
                text = "Welcome to connect !"
            });
        }

        protected override void OnDisConnected()
        {
            PETool.LogMsg("Client DisConnect !!!");
        }

        protected override void OnReciveMsg(NetMsg msg)
        {
            PETool.LogMsg("Client Req:" + msg.text);
            SendMsg(new NetMsg
            {
                text = "Sever Rsp:"+ msg.text
            });
        }

    }
}
