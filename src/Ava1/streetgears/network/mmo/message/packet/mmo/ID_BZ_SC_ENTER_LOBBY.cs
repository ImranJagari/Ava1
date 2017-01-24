using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class ID_BZ_SC_ENTER_LOBBY : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;
        public static Int32 unk4;
        public static Int32 unk5;
        public static Int32 unk6;
        public static Int32 unk7;
        public static Int32 unk8;

        public ID_BZ_SC_ENTER_LOBBY()
        {

        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.ID_BZ_SC_ENTER_LOBBY + 16744);

            message.PutString("SUCCESS\0");
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutInt(unk4);
            message.PutInt(unk5);
            message.PutInt(unk6);
            message.PutInt(unk7);
            message.PutInt(unk8);

            return MessageManager.Build(message, true);
        }
    }
}
