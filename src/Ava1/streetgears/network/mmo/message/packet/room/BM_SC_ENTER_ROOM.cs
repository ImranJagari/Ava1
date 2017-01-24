using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.room
{
    public class BM_SC_ENTER_ROOM : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;
        public static String roomIp;
        public static Int16 roomPort;
        public static SByte team;
        public static SByte unk8;
        public static SByte unk9;
        public static Int32 udpPort;

        public BM_SC_ENTER_ROOM(string _roomIp, short _roomPort, sbyte _team, int _udpPort)
        {
            roomIp = _roomIp;
            roomPort = _roomPort;
            team = _team;
            _udpPort = udpPort;
        }

        public override byte[] Pack()
        {
            var message = new Message(LobbyServerEnums.BM_SC_ENTER_ROOM + 1);

            message.PutString(successCmd);
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutString(roomIp.PadRight(20, '\0'));
            message.PutShort(roomPort);
            message.PutByte((byte)team);
            message.PutByte((byte)unk8);
            message.PutByte((byte)unk9);
            message.PutInt(udpPort);

            return MessageManager.Build(message, true);
        }
    }
}
