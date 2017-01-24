using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.social
{
    public class BM_SC_CHAT_USER : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;

        public BM_SC_CHAT_USER()
        {
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_CHAT_USER + 1);

            message.PutString("SUCCESS\0");
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);

            return MessageManager.Build(message, true);
        }
    }
}
