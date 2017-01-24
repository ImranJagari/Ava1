using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.room
{
    public class BM_SC_UNKNOW_INFO : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String charName;

        public BM_SC_UNKNOW_INFO(string _charName)
        {
            charName = _charName;
        }

        public override byte[] Pack()
        {
            var message = new Message(LobbyServerEnums.BM_SC_UNKNOW_INFO + 1);

            message.PutString(charName.PadRight(40, '\0'));

            return MessageManager.Build(message, true);
        }
    }
}
