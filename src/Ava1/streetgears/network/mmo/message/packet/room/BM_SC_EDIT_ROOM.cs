using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.room
{
    public class BM_SC_EDIT_ROOM : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";

        public BM_SC_EDIT_ROOM()
        {
        }

        public override byte[] Pack()
        {
            var message = new Message((ushort)LobbyServerEnums.BM_SC_EDIT_ROOM + 1);

            message.PutString(successCmd);
            message.PutInt(0);
            message.PutInt(0);
            message.PutInt(0);
            message.PutInt(0);
            message.PutInt(0);
            message.PutInt(0);
            message.PutInt(0);
            message.PutInt(0);
            message.PutInt(0);

            return MessageManager.Build(message, true);
        }
    }
}
