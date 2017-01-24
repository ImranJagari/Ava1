using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.auth.message.packet
{
    public class TM_SC_SELECT_SERVER : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static SByte status;

        public TM_SC_SELECT_SERVER(sbyte _status)
        {
            status = _status;
        }

        public override byte[] Pack()
        {
            var message = new Message(AuthServerEnums.TM_SC_SELECT_SERVER + 1);

            message.PutByte((byte)status);

            return MessageManager.Build(message, false);
        }
    }
}
