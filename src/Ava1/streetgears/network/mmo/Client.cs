using System;
using System.Linq;
using System.Reflection;

using Ava1.shared.database.account;
using Ava1.shared.core.network;
using Ava1.lib.core.io;
using Ava1.lib.core.utils;
using Ava1.lib.core.config;

namespace Ava1.streetgears.network.mmo
{
    public class Client
    {
        private BaseClient _client { get; set; }
        public Account account { get; set; }

        public string sessionKey { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }

        public Client(BaseClient client)
        {
            _client = client;
            Ip = client.ip;
            Port = client.prt;
            client_OnClientSocketConnected();
            client.OnClientSocketClosed += client_OnClientSocketClosed;
            client.OnClientReceivedData += client_OnClientReceivedData;
        }

        private void client_OnClientSocketConnected()
        {
        }

        private void client_OnClientSocketClosed()
        {
            var server = shared.database.server.Server.Servers.FirstOrDefault(x => x.name == Config.elements["Server"]["Name"]);
            server.channel_currentnum--;
            server.UpdateServer();

            //Log.WriteLog(Log.Type.Info, "Client '{0}:{1}' left mmo server", _client.ip, _client.prt);
            Disconnect();
        }

        public void Disconnect()
        {
            this.account?.UpdateAccount();
            Server.clients?.Remove(this);
            _client?.Close();
        }

        private void client_OnClientReceivedData(byte[] data)
        {
            MethodInfo methodToInvok;
            try
            {
                if (data.Length > 4)
                {
                    var message = new Message(data, 2);
                    if (mmo.message.Manager.MethodHandlers.TryGetValue((ushort)message.Op, out methodToInvok))
                    {
                        //console.cw(console.type.Recv, "'{0}:{1}' received {0}".Replace("Handle", string.Empty), _client.ip, _client.prt, methodToInvok.Name);
                    }
                    else
                    {
                        //console.cw(console.type.Send, "Received unknow packet '{0}' from '{1}:{2}'", Functions.ByteArrayToString(data), _client.ip, _client.prt);
                    }
                    mmo.message.Manager.Handle(this, message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void Send(PacketBase packet)
        {
            MethodInfo methodToInvok;

            try
            {
                if (this != null)
                {
                    _client.Send(packet.Pack());
                }

                if (message.Manager.MethodHandlers.TryGetValue((ushort)packet.packetOp, out methodToInvok))
                {
                    //console.cw(console.type.Send, "Sent {0}{1} to '{1}:{2}'", methodToInvok.Name, methodToInvok.GetParameters(), _client.ip, _client.prt);
                }
                else
                {
                    //console.cw(console.type.Send, "Unknow packet '{0}' sent to '{1}:{2}'", Functions.ByteArrayToString(packet.Pack()), _client.ip, _client.prt);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(Log.Type.Error, ex.ToString());
            }
        }
    }
}
