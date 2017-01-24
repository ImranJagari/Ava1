using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BM_SC_INVENTORY : PacketBase
    {
        public static Client client;

        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;
        public static SByte unk4;
        public static Int16 count;
        public static Int32 numberId;
        public static Int32 itemId;
        public static Int32 tradeStatus;
        public static Int16 unk5;
        public static Int32 duration;
        public static Int32 unk6;
        public static Int16 unk7;
        public static Int16 equiped;
        public static Int32 unk8;

        public BM_SC_INVENTORY(Client _client)
        {
            client = _client;
        }

        public void PutItem(Message message, int numberId, int itemId, int tradeStatus, int duration, sbyte equiped, int unk3, short unk0 = 0, int unk1 = 0, short unk2 = 0)
        {
            message.PutInt(numberId);
            message.PutInt(itemId);
            message.PutInt(tradeStatus);
            message.PutShort(unk0);
            message.PutInt(duration);
            message.PutInt(unk1);
            message.PutShort(unk2);
            message.PutByte((byte)equiped);
            message.PutInt(unk3);
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_INVENTORY + 1);

            count = (short)client.account.Item.Count;

            message.PutString(successCmd);
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);   
            message.PutInt(unk3);
            message.PutByte((byte)unk4);
            message.PutShort(count);

            if (client.account.Item.Count > 0)
            {
                for (int i = 0; i < client.account.Item.Count; i++)
                {
                    PutItem(message, client.account.Item[i].numberId, client.account.Item[i].itemId, client.account.Item[i].tradeStatus, client.account.Item[i].duration, (sbyte)client.account.Item[i].equiped, 0);
                }
            }

            return MessageManager.Build(message, true);
        }
    }
}
