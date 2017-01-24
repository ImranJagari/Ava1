using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BM_SC_ENTER_CHANNEL : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;
        public static Byte channel_id;

        public BM_SC_ENTER_CHANNEL(byte _channel_id)
        {
            channel_id = _channel_id;
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_ENTER_CHANNEL + 1);

            message.PutString(successCmd);
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutByte(channel_id);
            message.PutByte(channel_id);

            return MessageManager.Build(message, true);
        }
    }
}
