using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BM_SC_PLAYER_CHARACTER_LIST : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;
        public static Byte unk4;
        public static String char_name = "";
        public static Int16 first_login;
        public static Int32 unk5;
        public static Int32 char_type;

        public BM_SC_PLAYER_CHARACTER_LIST(string _char_name, short _first_login, int _char_type)
        {
            char_name = _char_name;
            char_type = _char_type;
            first_login = _first_login;
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_PLAYER_CHARACTER_LIST + 1);

            message.PutString(successCmd);
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutByte(unk4);
            message.PutString(char_name.PadRight(43, '\0'));
            message.PutShort(first_login);
            message.PutInt(unk5);
            message.PutInt(char_type);

            return MessageManager.Build(message, true);
        }
    }
}
