using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.social
{
    public class BM_SC_CHAT_MESSAGE : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public Int32 unk0;
        public Int32 unk1;
        public Int32 unk2;
        public Int32 unk3;
        public Int32 unk4;
        public Int32 unk5;
        public Int32 unk6;
        public Int32 unk7;
        public Byte unk8;
        public String msgSender;
        public Int32 unk9;
        public Int32 unk10;
        public SByte msgType;
        public Int16 msgLenght;
        public String msg;

        public BM_SC_CHAT_MESSAGE(string _msgSender, sbyte _msgType, short _msgLenght, string _msg)
        {
            msgSender = _msgSender;
            msgType = _msgType;
            msgLenght = _msgLenght;
            msg = _msg;
        }

        public override byte[] Pack()
        {
            var message = new Message((UInt16)MmoServerEnums.BM_SC_CHAT_MESSAGE);

            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutInt(unk4);
            message.PutInt(unk5);
            message.PutInt(unk6);
            message.PutInt(unk7);
            message.PutByte(unk8);
            message.PutString(msgSender.PadRight(32, '\0'));
            message.PutInt(unk9);
            message.PutInt(unk10);
            message.PutByte((byte)msgType);
            message.PutShort(msgLenght);
            message.PutString(msg);

            return MessageManager.Build(message, true);
        }
    }
}
