using System;

using Ava1.streetgears.network.relay.message.packet;
using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.relay.message.handlers
{
    public class HandleGlobalRelayInteractions
    {
        [Relay((UInt16)RelayServerEnums.NM_SC_READY_GAME)]
        public void HandleReadyGame(Client client, Message message)
        {
            client.Send(new NM_SC_READY_GAME());
        }

        [Relay((UInt16)RelayServerEnums.NM_SC_START_GAME)]
        public void HandleStartGame(Client client, Message message)
        {
            client.Send(new NM_SC_START_GAME());
        }

        [Relay((UInt16)RelayServerEnums.NM_SC_START_GAME2)]
        public void HandleStartGame2(Client client, Message message)
        {
            client.Send(new NM_SC_START_GAME2());
        }

        [Relay((UInt16)RelayServerEnums.NM_SC_EXPIRE)]
        public void HandleExpire(Client client, Message message)
        {
            client.Send(new NM_SC_EXPIRE());
        }

        [Relay((UInt16)RelayServerEnums.NM_SC_PLAYER_INFO)]
        public void HandlePlayerInfo(Client client, Message message)
        {
            client.Send(new NM_SC_PLAYER_INFO());
        }

        [Relay((UInt16)RelayServerEnums.NM_SC_GET_SERVER_TICK)]
        public void HandleServerTick(Client client, Message message)
        {
            client.Send(new NM_SC_GET_SERVER_TICK());
        }
    }
}
