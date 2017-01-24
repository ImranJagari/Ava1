using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.room
{
    public class BM_SC_LEAVE_ROOM : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCESS\0";
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;

        public BM_SC_LEAVE_ROOM()
        {

        }

        public override byte[] Pack()
        {
            var message = new Message(LobbyServerEnums.BM_SC_LEAVE_ROOM + 1);

            message.PutString(successCmd);
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);

            return MessageManager.Build(message, true);
        }
    }
}
