using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BM_SC_REWARD_QUEST : PacketBase
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
        public static Int32 unk9;
        public static Int16 unk10;
        public static Int32 sp;
        public static Int32 exp;
        public static Int32 rupees;
        public static Int32 coins;
        public static Int32 unk11;
        public static Int32 unk12;

        public BM_SC_REWARD_QUEST(int _sp, int _exp, int _rupees, int _coins)
        {
            sp = _sp;
            exp = _exp;
            rupees = _rupees;
            coins = _coins;
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_REWARD_QUEST + 1);

            message.PutString(successCmd);
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutInt(unk4);
            message.PutInt(unk5);
            message.PutInt(unk6);
            message.PutInt(unk7);
            message.PutInt(unk8);
            message.PutInt(unk9);
            message.PutShort(unk10);
            message.PutInt(sp); // eax, [esi+37h]
            message.PutInt(exp); // edx, [esi+3Bh]
            message.PutInt(rupees); // ecx, [esi+3Fh]
            message.PutInt(coins); //  eax, [esi+43h]
            message.PutInt(unk11); //  edx, [esi+47h]
            message.PutInt(unk12);

            return MessageManager.Build(message, true);
        }
    }
}
