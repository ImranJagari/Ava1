using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BS_SC_EXPAND_SLOT : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";
        public static Int32 unk0;

        public BS_SC_EXPAND_SLOT()
        {
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BS_SC_EXPAND_SLOT + 1);

            message.PutString(successCmd);
            message.PutInt(unk0);

            return MessageManager.Build(message, true);
        }
    }
}
