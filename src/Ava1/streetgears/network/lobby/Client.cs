using Ava1.shared.database.account;
using Ava1.shared.core.network;
using Ava1.lib.core.utils;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.lobby
{
    public class Client
    {
        private BaseClient _client { get; set; }
        public Account account { get; set; }

        public string sessionKey { get; set; }

        public Client(BaseClient client)
        {
            _client = client;
            client_OnClientSocketConnected();
            client.OnClientSocketClosed += client_OnClientSocketClosed;
            client.OnClientReceivedData += client_OnClientReceivedData;
        }

        private void client_OnClientSocketClosed()
        {
            Disconnect();

            //Log.WriteLog(Log.Type.Info, "Client {0}:{1} left lobby server", _client.ip, _client.prt);
        }

        public void Disconnect()
        {
            Server.clients.Remove(this);
            _client.Close();
        }

        private void client_OnClientSocketConnected() { }

        private void client_OnClientReceivedData(byte[] data) { }

        public void Send(PacketBase packet) { }
    }
}
