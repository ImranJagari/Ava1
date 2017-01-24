using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace  Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BM_SC_BALANCE_INFO : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;
        public static Int32 gamecash;
        public static Int32 coin;
        public static Int32 cash;
        public static Int32 questpoints;

        public BM_SC_BALANCE_INFO(int _gamecash, int _coin, int _cash, int _questpoints)
        {
            gamecash = _gamecash;
            coin = _coin;
            cash = _cash;
            questpoints = _questpoints;
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_BALANCE_INFO + 1);

            message.PutString(successCmd);
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutInt(gamecash);
            message.PutInt(coin);
            message.PutInt(cash);
            message.PutInt(questpoints);

            return MessageManager.Build(message, true);
        }
    }
}
