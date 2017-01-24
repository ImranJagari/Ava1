using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.room
{
    public class BM_SC_USER_INFO : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String charIp;
        public static String charName;
        public static SByte lobbyPosition;
        public static SByte charType;
        public static SByte entryInfo;
        public static SByte isAdmin;
        public static SByte slotDisplay;
        public static SByte isReady;
        public static SByte status;
        public static Int16 unk0;
        public static SByte unk1;
        public static SByte unk2 = 1;

        public BM_SC_USER_INFO(string _charIp, string _charName, sbyte _lobbyPosition, sbyte _charType, sbyte _entryInfo, sbyte _isAdmin, sbyte _slotDisplay, sbyte _isReady, sbyte _status)
        {
            charIp = _charIp;
            charName = _charName;
            lobbyPosition = _lobbyPosition;
            charType = _charType;
            entryInfo = _entryInfo;
            isAdmin = _isAdmin;
            slotDisplay = _slotDisplay;
            isReady = _isReady;
            status = _status;
        }

        public override byte[] Pack()
        {
            var message = new Message(LobbyServerEnums.BM_SC_USER_INFO + 1);

            message.PutString(charIp.PadRight(33, '\0'));
            message.PutString(charName.PadRight(40, '\0'));
            message.PutByte((byte)lobbyPosition);
            message.PutByte((byte)charType);
            message.PutByte((byte)entryInfo);
            message.PutByte((byte)isAdmin);
            message.PutByte((byte)slotDisplay);
            message.PutByte((byte)isReady);
            message.PutByte((byte)status);
            message.PutShort(unk0);
            message.PutByte((byte)unk1);
            message.PutByte((byte)unk2);

            return MessageManager.Build(message, true);
        }
    }
}
