using System;
using System.Linq;

using Ava1.streetgears.network.mmo.message.packet.mmo;
using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;
using Ava1.lib.core.utils;
using Ava1.lib.core.config;

namespace Ava1.streetgears.network.mmo.message.handlers
{
    public class HandleChannelInteractions
    {
        [MMO((UInt16)MmoServerEnums.BM_SC_LEAVE_CHANNEL)]
        public static void HandleChannelExit(Client client, Message message)
        {
            client.Send(new BM_SC_LEAVE_CHANNEL());

            client.account.isInGame = false;

            Log.WriteLog(Log.Type.Info, "{0} left ParkTown", client.account.char_name);
        }

        public static void HandleChannelEntry(Client client, Message message)
        {
            var server = shared.database.server.Server.Servers.FirstOrDefault(x => x.name == Config.elements["Server"]["Name"]);
            if (server != null)
            {
                if (!client.account.isInGame)
                {
                    client.Send(new BM_SC_ENTER_CHANNEL((byte)1));

                    HandleGameLogin.HandleBalanceInfo(client, message);
                    HandleGameLogin.HandlePlayerLevelInfo(client, message);

                    client.account.isInGame = true;

                    Log.WriteLog(Log.Type.Info, "{0} joined ParkTown", client.account.char_name);
                }
            }
        }

        public static void HandleChannelList(Client client, Message message)
        {
            var server = shared.database.server.Server.Servers.FirstOrDefault(x => x.name == Config.elements["Server"]["Name"]);
            if (server != null)
            {
                if (!client.account.isInGame)
                {
                    client.Send(new BM_SC_GET_CHANNEL_LIST(1, (byte)server.id, server.name, server.channel_currentnum, server.channel_maxnum));

                    HandleChannelEntry(client, message);
                }
            }
        }
    }
}
