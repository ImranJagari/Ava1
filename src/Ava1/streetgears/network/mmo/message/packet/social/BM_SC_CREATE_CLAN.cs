using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.social
{
    public class BM_SC_CREATE_CLAN : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;
        public static String clanName;

        public BM_SC_CREATE_CLAN(string _clanName)
        {
            clanName = _clanName;
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_CREATE_CLAN + 1);

            message.PutString(successCmd);
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutString(clanName.PadRight(20, '\0').Replace("\0", string.Empty));

            return MessageManager.Build(message, true);
        }
    }
}
