using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BM_SC_ITEM_USE : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;
        public static Int32 itemSlotNumber;
        public static Int16 itemCount;

        public BM_SC_ITEM_USE(int _itemSlotNumber, short _itemCount)
        {
            itemSlotNumber = _itemSlotNumber;
            itemCount = _itemCount;
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_ITEM_USE + 1);

            message.PutString(successCmd);
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutInt(itemSlotNumber);
            message.PutShort(itemCount);

            return MessageManager.Build(message, true);
        }
    }
}
