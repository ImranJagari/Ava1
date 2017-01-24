using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BM_SC_MINIGAME_FINISH : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static Int32 unk0;
        public static SByte unk1;
        public static Int32 unk2;
        public static Int32 unk3;

        public BM_SC_MINIGAME_FINISH()
        {

        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_MINIGAME_FINISH + 1);

            message.PutInt(41504);
            message.PutByte(1);
            message.PutInt(41504);
            message.PutInt(41504);

            return MessageManager.Build(message, true);
        }
    }
}
