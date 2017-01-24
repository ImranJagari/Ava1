using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.auth.message.packet
{
    public class TS_SC_WE_LOGIN : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static Int16 unk0;
        public static Byte unk1;
        public static String sessionKey = null;
        public static Int32 unk2;
        public static Int32 unk3;
        public static Int32 unk4;
        public static Int32 unk5;
        public static Int16 unk6 = 700;
        public static Int16 unk7;
        public static Byte unk8 = 1;

        public TS_SC_WE_LOGIN(string _sessionKey)
        {
            sessionKey = _sessionKey;
        }

        public override byte[] Pack()
        {
            var message = new Message((UInt16)AuthServerEnums.TS_SC_WE_LOGIN);

            message.PutShort(unk0);
            message.PutByte(unk1); 
            message.PutString(sessionKey);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutInt(unk4);
            message.PutInt(unk5);
            message.PutShort(unk6);
            message.PutShort(unk7);
            message.PutByte(unk8);

            return MessageManager.Build(message, false);
        }
    }
}
