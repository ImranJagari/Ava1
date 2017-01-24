using System;
using System.Linq;

using Ava1.streetgears.network.mmo.message.packet.mmo;
using Ava1.streetgears.network.mmo.message.handlers;
using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.handlers
{
    public class HandleLobbyInteractions
    {
        [MMO((UInt16)MmoServerEnums.BM_SC_PLAYER_CHARACTER_INFO)]
        public static void HandleCharacterInfo(Client client, Message message)
        {
            var charName = message.GetString(40).Replace("\0", string.Empty);
            var unk0 = message.GetByte();

            var thisClient = Server.clients.FirstOrDefault(x => x.account.char_name == charName);
            var thisRoom = HandleRoomInteractions.RoomParty.FirstOrDefault(x => x.Client.Contains(thisClient));

            if (thisRoom != null)
            {
                thisRoom.Send(new BM_SC_PLAYER_CHARACTER_INFO(thisClient, (sbyte)thisClient.account.char_type, thisClient.account.char_name, thisClient.account.char_level));
            }
        }
    }
}
