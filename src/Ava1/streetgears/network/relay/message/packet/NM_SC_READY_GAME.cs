using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.relay.message.packet
{
    public class NM_SC_READY_GAME : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";

        public NM_SC_READY_GAME()
        {
        }

        public override byte[] Pack()
        {
            var message = new Message((UInt16)RelayServerEnums.NM_SC_READY_GAME);

            message.PutString(successCmd);

            return MessageManager.Build(message, true);
        }
    }
}
