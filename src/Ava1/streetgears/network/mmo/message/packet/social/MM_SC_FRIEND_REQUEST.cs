using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class MM_SC_FRIEND_REQUEST : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";

        public MM_SC_FRIEND_REQUEST()
        {

        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.MM_SC_FRIEND_REQUEST + 1);

            message.PutString(successCmd);

            return MessageManager.Build(message, true);
        }
    }
}
