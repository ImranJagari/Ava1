using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.social
{
    public class BM_SC_UPDATE_MYMATEINFO : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "mate_success";
        public static Int32 unk0;

        public BM_SC_UPDATE_MYMATEINFO()
        {

        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_UPDATE_MYMATEINFO + 1);

            message.PutString(successCmd);
            message.PutInt(unk0);

            return MessageManager.Build(message, true);
        }
    }
}
