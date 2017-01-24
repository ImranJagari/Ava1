using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.auth.message.packet
{
    public class TM_SC_RESULT : PacketBase
    {
        public override UInt16 packetOp {get; set; }
        public static Byte unk0;
        public static Byte resultId;

        public TM_SC_RESULT(byte _resultId)
        {
            resultId = _resultId;
        }

        public override byte[] Pack()
        {
            var message = new Message((UInt16)AuthServerEnums.TM_SC_RESULT);

            message.PutByte(unk0);
            message.PutByte(resultId);

            return MessageManager.Build(message, false);
        }
    }
}
