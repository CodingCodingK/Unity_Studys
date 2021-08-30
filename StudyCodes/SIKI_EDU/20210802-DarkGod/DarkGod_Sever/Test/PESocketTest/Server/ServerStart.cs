using System;
using PENet;
using Protocol;

namespace Server
{
    class ServerStart
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ServerStart!");
            PESocket<ServerSession, NetMsg> server = new PESocket<ServerSession, NetMsg>();

            server.StartAsServer(IpCfg.svrIP,IpCfg.svrPort);

            while(true){}
        }
    }

}