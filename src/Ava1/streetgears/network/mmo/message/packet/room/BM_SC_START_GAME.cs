using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.config;
using Ava1.lib.core.io;
using Ava1.lib.core.utils;

namespace Ava1.streetgears.network.mmo.message.packet.room
{
    public class BM_SC_START_GAME : PacketBase
    {
        public override UInt16 packetOp { get; set; }

        public static engine.room.RoomParty thisRoom;

        public static String successCmd = "SUCCESS\0";
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;
        public static Int32 unk4;
        public static Int32 unk5;
        public static Int32 unk6;
        public static Int32 unk7;
        public static String encryptionKey;
        public static Int16 playerCount;
        public static String[] charIp = new string[8] { "", "", "", "", "", "", "", ""};

        public static Int32 unk8;
        public static Int32 unk9;
        public static Int32 unk10;
        public static Int32 unk11;
        public static Int32 unk12;
        public static Int32 unk13;
        public static Int32 unk14;
        public static Int32 unk15;
        public static Int16 unk16;
        public static String charName;
        public static Int16 unk17 = 16;
        public static String remoteEndPoint;
        public static Int32 unk18 = 1;
        public static Int32 unk19 = 1;
        public static Int16 index;
        public static SByte unk20 = 1;
        public static SByte isLeader;

        public BM_SC_START_GAME(engine.room.RoomParty _thisRoom, string _encryptionKey, short _playerCount)
        {
            thisRoom = _thisRoom;
            playerCount = _playerCount;
            encryptionKey = _encryptionKey;

            for(int i = 1; i < playerCount; i++)
            {
                charIp[i] = thisRoom.Client[i].Ip;
            }
        }

        public void PutPlayer(Message message, string charName, string remoteEndPoint, short index, sbyte isLeader)
        {
            message.PutInt(0);
            message.PutInt(0);
            message.PutInt(0);
            message.PutInt(0);
            message.PutInt(0);
            message.PutInt(0);
            message.PutInt(0);
            message.PutInt(0);
            message.PutShort(0);
            message.PutString(charName.PadRight(40, '\0'));
            message.PutShort((short)remoteEndPoint.Length); 
            message.PutString(remoteEndPoint.PadRight(remoteEndPoint.Length + 1, '\0'));
            message.PutInt(1);
            message.PutInt(1);
            message.PutShort(index);
            message.PutByte(1);
            message.PutByte((byte)isLeader);
        }

        public override byte[] Pack()
        {
            var message = new Message(LobbyServerEnums.BM_SC_START_GAME + 1);

            message.PutString(successCmd);
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutString(encryptionKey.PadRight(16, '\0'));
            message.PutShort(playerCount);

            for(int i = 1; i < playerCount; i++)
            {
                message.PutString(charIp[i].PadRight(45, '\0'));
            }

            foreach(Client client in thisRoom.Client)
            {
                PutPlayer(message, client.account.char_name, client.Ip, (short)client.account.roomSlot, Convert.ToSByte(client.account.isLeader));
            }

            return MessageManager.Build(message, true);
        }
    }
}
