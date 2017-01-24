using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BM_SC_DELETE_ITEM_RESULT : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String resultCmd;
        public static Int32 unk0;

        public BM_SC_DELETE_ITEM_RESULT(string _resultCmd)
        {
            resultCmd = _resultCmd;
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_DELETE_ITEM + 1);

            message.PutString(resultCmd);
            message.PutInt(unk0);

            return MessageManager.Build(message, true);
        }
    }
}
