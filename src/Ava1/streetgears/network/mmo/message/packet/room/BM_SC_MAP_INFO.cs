using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.room
{
    public class BM_SC_MAP_INFO : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static Int16 mapId;

        public BM_SC_MAP_INFO(short _mapId)
        {
            mapId = _mapId;
        }

        public override byte[] Pack()
        {
            var message = new Message(LobbyServerEnums.BM_SC_MAP_INFO + 1);

            message.PutShort(mapId);

            return MessageManager.Build(message, true);
        }
    }
}
