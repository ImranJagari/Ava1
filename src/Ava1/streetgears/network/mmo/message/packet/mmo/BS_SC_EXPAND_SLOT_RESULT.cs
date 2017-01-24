using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BS_SC_EXPAND_SLOT_RESULT : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String result;

        public BS_SC_EXPAND_SLOT_RESULT(string _result)
        {
            result = _result;
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BS_SC_EXPAND_SLOT + 1);

            message.PutString(result);

            return MessageManager.Build(message, true);
        }
    }
}
