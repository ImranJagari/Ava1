using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BM_SC_MINIGAME_START : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static Int32 unk0;
        public static Int32 unk1;
        public static String charName;
        public static Int32 unk3;

        public BM_SC_MINIGAME_START(string _charName)
        {
            charName = _charName;
        }

        public override byte[] Pack()
        {   
            var message = new Message(MmoServerEnums.BM_SC_MINIGAME_START + 1);

            message.PutInt(1);
            message.PutInt(41505);
            message.PutString(charName.PadRight(40, '\0'));
            message.PutInt(41505);

            return MessageManager.Build(message, true);
        }
    }
}
