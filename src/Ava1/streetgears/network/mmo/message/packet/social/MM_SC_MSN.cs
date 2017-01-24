using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.social
{
    public class MM_SC_MSN : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;
        public static Int32 unk4 = 1;
        public static Int32 unk5 = 1;

        public MM_SC_MSN()
        {            
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.MM_SC_MSN + 1);

            message.PutString(successCmd);
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutInt(unk4);
            message.PutInt(unk5);

            return MessageManager.Build(message, true);
        }
    }
}
