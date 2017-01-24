using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BM_SC_CREATE_CHAR : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";

        public BM_SC_CREATE_CHAR()
        {

        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_LOGIN + 1);

            message.PutString(successCmd);

            return MessageManager.Build(message, true);
        }
    }
}
