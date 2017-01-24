using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.social
{
    public class BM_SC_CREATE_CLAN_RESULT : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String resultCmd;
        public static Int32 unk0;

        public BM_SC_CREATE_CLAN_RESULT(string _resultCmd)
        {
            resultCmd = _resultCmd;
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_CREATE_CLAN + 1);

            message.PutString(resultCmd);
            message.PutInt(unk0);

            return MessageManager.Build(message, true);
        }
    }
}
