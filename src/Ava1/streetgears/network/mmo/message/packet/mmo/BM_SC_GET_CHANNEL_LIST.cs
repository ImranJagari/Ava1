using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BM_SC_GET_CHANNEL_LIST : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;
        public static Int16 channel_count;
        public static Byte channel_id;
        public static String channel_name;
        public static Int32 unk4 = 5;
        public static Int32 unk5 = 6;
        public static Int32 unk6 = 7;
        public static Int32 channel_players;
        public static Int32 channel_maximumPlayers;

        public BM_SC_GET_CHANNEL_LIST(short _channel_count, byte _channel_id, string _channel_name, int _channel_players, int _channel_maximumPlayers)
        {
            channel_count = _channel_count;
            channel_id = _channel_id;
            channel_name = _channel_name;
            channel_players = _channel_players;
            channel_maximumPlayers = _channel_maximumPlayers;
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_GET_CHANNEL_LIST + 1);

            message.PutString(successCmd);
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutShort(channel_count);
            message.PutByte(channel_id);
            message.PutString(channel_name.PadRight(12, '\0'));
            message.PutInt(unk4);
            message.PutInt(unk5);
            message.PutInt(unk6);
            message.PutInt(channel_players);
            message.PutInt(channel_maximumPlayers);

            return MessageManager.Build(message, true);
        }
    }
}
