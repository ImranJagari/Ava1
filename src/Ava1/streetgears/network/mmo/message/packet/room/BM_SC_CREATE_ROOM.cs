using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.room
{
    public class BM_SC_CREATE_ROOM : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;
        public static Int32 roomId;
        public static String relayIp;
        public static Int16 relayPort;
        public static SByte team;
        public static SByte unk8;
        public static SByte unk9;
        public static Int32 udpPort = 5000;

        public BM_SC_CREATE_ROOM(int _roomId, string _relayIp, short _relayPort, sbyte _team, int _udpPort)
        {
            roomId = _roomId;
            relayIp = _relayIp;
            relayPort = _relayPort;
            team = _team;
            _udpPort = udpPort;
        }

        public override byte[] Pack()
        {
            var message = new Message((ushort)LobbyServerEnums.BM_SC_CREATE_ROOM + 1);

            message.PutString(successCmd);
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutInt(roomId);
            message.PutString(relayIp.PadRight(20, '\0'));
            message.PutShort(relayPort);
            message.PutByte((byte)team);
            message.PutByte((byte)unk8);
            message.PutByte((byte)unk9);
            message.PutInt(udpPort);

            return MessageManager.Build(message, true);
        }
    }
}
