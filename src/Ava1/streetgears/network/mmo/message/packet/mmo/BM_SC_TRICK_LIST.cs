using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BM_SC_TRICK_LIST : PacketBase
    {
        public static Client client;

        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;
        public static Byte unk4;
        public static Int16 trickCount = 13;
        public static Int32 trickId;
        public static Int32 trickLevel;
        public static Byte applyTrick;

        public BM_SC_TRICK_LIST(Client _client)
        {
            client = _client;
        }

        public void PutTrick(Message message, int id, int level, byte apply)
        {
            message.PutInt(id);
            message.PutInt(level);
            message.PutByte(apply);
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_TRICK_LIST + 1);

            message.PutString(successCmd);
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutByte(unk4);
            message.PutShort(trickCount);

            for (int i = 0; i < trickCount; i++)
            {
                PutTrick(message, client.account.Trick[i].Id, client.account.Trick[i].Level, client.account.Trick[i].Apply);
            }

            return MessageManager.Build(message, true);
        }
    }
}
