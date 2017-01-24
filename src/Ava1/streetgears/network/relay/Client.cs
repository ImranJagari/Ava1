using System;

using Ava1.lib.core.enums.network.op;
using Ava1.shared.core.network;
using Ava1.shared.database.account;
using Ava1.lib.core.utils;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.relay
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
            //Log.WriteLog(Log.Type.Info, "Client '{0}:{1}' left relay server", _client.ip, _client.prt);
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
            try
            {
                if (data.Length > 4)
                {
                    var message = new Message(data, 2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void Send(PacketBase packet)
        {
            try
            {
                if (this != null)
                {
                    _client.Send(packet.Pack());
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(Log.Type.Error, ex.ToString());
            }
        }
    }
}
