using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;
using Ava1.streetgears.network.mmo.message.handlers;

namespace Ava1.streetgears.network.mmo.message.packet.room
{
    public class BM_SC_GET_ROOM_LIST : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static Int16 roomCount;
        public static Int16 unk0;
        public static Int16 unk1;
        public static Int32 id;
        public static String name;
        public static Int32 mode;
        public static SByte currentPlayers;
        public static SByte maxPlayers;
        public static SByte state;
        public static SByte license;

        public BM_SC_GET_ROOM_LIST(short _roomCount)
        {
            roomCount = _roomCount;
        }

        public void PutRoom(Message message, int id, string name, int mode, sbyte currentPlayers, sbyte maxPlayers, sbyte state, sbyte license)
        {
            message.PutInt(id);
            message.PutString(name.PadRight(24, '\0'));
            message.PutInt(mode);
            message.PutByte((byte)currentPlayers);
            message.PutByte((byte)maxPlayers);
            message.PutByte((byte)state);
            message.PutByte((byte)license);
        }

        public override byte[] Pack()
        {
            var message = new Message(LobbyServerEnums.BM_SC_GET_ROOMLIST + 1);

            message.PutShort(roomCount);
            message.PutShort(unk0);
            message.PutShort(unk1);

            if (roomCount > 0)
            {
                for(int i = 0; i < roomCount; i++)
                {
                    if (HandleRoomInteractions.RoomList[i].currentPlayers != 0)
                    {
                        PutRoom(message, HandleRoomInteractions.RoomList[i].Id, HandleRoomInteractions.RoomList[i].Name, HandleRoomInteractions.RoomList[i].Mode, HandleRoomInteractions.RoomList[i].currentPlayers, HandleRoomInteractions.RoomList[i].maxPlayers, HandleRoomInteractions.RoomList[i].State, HandleRoomInteractions.RoomList[i].License);
                    }
                }
            }

            return MessageManager.Build(message, true);
        }
    }
}
