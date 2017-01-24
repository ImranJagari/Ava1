using System;
using System.Collections.Generic;

using Ava1.shared.core.network;
using Ava1.shared.database.account;
using Ava1.lib.core.config;
using Ava1.lib.core.utils;

namespace Ava1.streetgears.network.lobby
{
    public class Server
    {
        private static BaseServer _server { get; set; }
        public static List<Client> clients = new List<Client>();

        public static void Start()
        {
            _server = new BaseServer(Config.elements["Lobby"]["Ip"], int.Parse(Config.elements["Lobby"]["Port"]));
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
            Log.WriteLog(Log.Type.Info, "Started lobby server on port '{0}'", int.Parse(Config.elements["Lobby"]["Port"]));
        }

        private static void server_OnServerAcceptedSocket(BaseClient socket)
        {
            clients.Add(new Client(socket));

            //Log.WriteLog(Log.Type.Info, "'{0}:{1}' connected to Lobby server", socket.ip, socket.prt);
        }
    }
}
