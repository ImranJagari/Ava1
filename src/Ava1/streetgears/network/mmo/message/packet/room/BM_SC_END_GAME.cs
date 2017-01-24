using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.room
{
    public class BM_SC_END_GAME : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";

        public BM_SC_END_GAME()
        {
        }

        public override byte[] Pack()
        {
            var message = new Message(LobbyServerEnums.BM_SC_END_GAME + 1);

            message.PutString(successCmd);

            return MessageManager.Build(message, true);
        }
    }
}
