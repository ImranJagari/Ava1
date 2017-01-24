using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.social
{
    public class MM_SC_MSN_FIND_USER : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";

        public MM_SC_MSN_FIND_USER()
        {
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.MM_SC_MSN_FIND_USER + 1);

            message.PutString(successCmd);

            return MessageManager.Build(message, true);
        }
    }
}
