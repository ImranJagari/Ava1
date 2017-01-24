using System;
using System.Collections.Generic;

using Ava1.lib.core.enums.network.global;
using Ava1.lib.core.utils;
using Ava1.shared.core.network;
using Ava1.lib.core.config;
using Ava1.streetgears.network.mmo.message.packet.social;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo
{
    public class Server
    {
        private static BaseServer _server { get; set; }
        public static List<Client> clients = new List<Client>();

        public static void Start()
        {
            _server = new BaseServer(Config.elements["MMO"]["Ip"], int.Parse(Config.elements["MMO"]["Port"]));
            _server.OnServerStarted += server_OnServerStarted;
            _server.OnServerAcceptedSocket += server_OnServerAcceptedSocket;
            _server.OnServerFailedToStart += server_OnServerFailedToStart;
            _server.Start();
        }

        private static void server_OnServerFailedToStart(Exception ex)
        {
            Log.WriteLog(Log.Type.Error, ex.ToString());
        }

        private static void server_OnServerStarted()
        {
            Log.WriteLog(Log.Type.Info, "Started mmo server on port '{0}", Config.elements["MMO"]["Port"]);
        }

        private static void server_OnServerAcceptedSocket(BaseClient socket)
        {
            clients.Add(new Client(socket));
            
            //Log.WriteLog(Log.Type.Info, "'{0}:{1}' connected to MMO server", socket.ip, socket.prt);
        }

        public static void Send(PacketBase packet)
        {
            clients.ForEach(x => x.Send(packet));
        }

        public static void Announce(string message)
        {
            clients.ForEach(x => x.Send(new BM_SC_CHAT_MESSAGE(string.Empty, (sbyte)ChatTypeEnums.ChatType.CHAT_SYSTEM_ANNOUNCEMENT, (short)message.Length, message)));
        }

        public static void Debug(string message)
        {
            clients.ForEach(x => x.Send(new BM_SC_CHAT_MESSAGE(string.Empty, (sbyte)ChatTypeEnums.ChatType.CHAT_DEBUG, (short)message.Length, message)));
        }
    }
}
