using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BM_SC_MMO_EVENT_MESSAGE : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk3;
        public static Int32 unk4;
        public static Int32 unk5;
        public static Int32 unk6;
        public static Int32 unk7;
        public static Int32 unk8;
        public static Int32 unk9;
        public static Int32 unkEvent;

        public BM_SC_MMO_EVENT_MESSAGE(int _unkEvent)
        {
            unkEvent = _unkEvent;
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_MMO_EVENT_MESSAGE + 1);

            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk3);
            message.PutInt(unk4);
            message.PutInt(unk5);
            message.PutInt(unk6);
            message.PutInt(unk7);
            message.PutInt(unk8);
            message.PutInt(unk9);
            message.PutInt(unkEvent);

            return MessageManager.Build(message, true);
        }
    }
}
