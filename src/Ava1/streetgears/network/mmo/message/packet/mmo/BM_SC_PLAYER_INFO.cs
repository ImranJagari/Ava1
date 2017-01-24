using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BM_SC_PLAYER_INFO : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static Int32 unk0 = 4;
        public static Int16 packetSize = 0x2D;
        public static String successCmd = "SUCCESS\0";
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;
        public static Int32 unk4;
        public static Int32 unk5;
        public static Int16 unk6;
        public static Int32 unk7 = 5;

        //Sub Packet One
        public static Int16 subPacketId = 1;
        public static Int16 subPacketSize = 50;
        public static String mapName = "ID1_Testo_2";
        public static Int16 unk8;
        public static Int32 unk9;
        public static Int32 unk10;
        public static Int32 unk11;
        public static Int32 unk12;
        public static Int32 unk13;
        public static Int32 unk14;
        public static Int32 unk15;
        public static String mapName2 = "ID1_Test";

        //Sub Packet Two
        public static Int16 subPacketSecondId = 2;
        public static Int16 subPacketSecondSize = 12;
        public static Int16 unk16 = 5;
        public static Int16 unk17 = 5;
        public static Int32 unk18 = 45;
        public static Int32 playerLevel;

        //Sub Packet Three
        public static Int16 subPacketThirdId = 4;
        public static Int16 subPacketThirdSize = 12;
        public static Int32 unk19 = 5;
        public static Int32 unk20 = 6;
        public static Int32 unk21 = 7;

        //Sub Packet Four
        public static Int16 subPacketFourthId = 64;
        public static Int16 subPacketFourthSize = 16;
        public static Int32 unk22 = 7;
        public static Int32 unk23 = 8;
        public static Int32 unk24 = 9;
        public static Int32 unk25 = 10;

        //Sub Packet Five
        public static Int16 subPacketFifthId = 512;
        public static Int16 subPacketFifthSize = 44;
        public static float speed;
        public static float accel;
        public static float turn;
        public static float brake;
        public static float boostSpeed;
        public static float overSpeed;
        public static float boosterC;
        public static float trick;
        public static float unk26;
        public static float unk27;

        //Sub Packet Six
        public static Int16 subPacketSixthId = 1024;
        public static Int16 subPacketSixthSize = 12;
        public static Int32 count1 = 2;
        public static Int32 count2 = 3;
        public static Int32 count3 = 3;
        public static Int32 count4 = 0;

        public BM_SC_PLAYER_INFO(Int32 _playerLevel)
        {
            playerLevel = _playerLevel;
        }

        public override byte[] Pack()
        {       
            var message = new Message(MmoServerEnums.BM_SC_PLAYER_INFO + 1);

            message.PutInt(unk0);
            message.PutShort(packetSize);
            message.PutString(successCmd);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutInt(unk4);
            message.PutInt(unk5);
            message.PutShort(unk6);
            message.PutInt(unk7);

            message.PutShort(subPacketId);
            message.PutShort(subPacketSize);
            message.PutString(mapName.PadRight(11, '\0'));
            message.PutShort(unk8);
            message.PutInt(unk9);
            message.PutInt(unk10);
            message.PutInt(unk11);
            message.PutInt(unk12);
            message.PutInt(unk13);
            message.PutInt(unk14);
            message.PutInt(unk15);
            message.PutString(mapName2.PadRight(9, '\0'));

            message.PutShort(subPacketSecondId);
            message.PutShort(subPacketSecondSize);
            message.PutShort(unk16);
            message.PutShort(unk17);
            message.PutInt(unk18);
            message.PutInt(playerLevel);

            message.PutShort(subPacketThirdId);
            message.PutShort(subPacketThirdSize);
            message.PutInt(unk19);
            message.PutInt(unk20);
            message.PutInt(unk21);

            message.PutShort(subPacketFourthId);
            message.PutShort(subPacketFourthSize);
            message.PutInt(unk22);
            message.PutInt(unk23);
            message.PutInt(unk24);
            message.PutInt(unk25);

            message.PutShort(subPacketFifthId);
            message.PutShort(subPacketFifthSize);
            message.PutFloat(speed);
            message.PutFloat(accel);
            message.PutFloat(turn);
            message.PutFloat(brake);
            message.PutFloat(boostSpeed);
            message.PutFloat(overSpeed);
            message.PutFloat(boosterC);
            message.PutFloat(trick);
            message.PutFloat(unk26);
            message.PutFloat(unk27);
            message.PutInt(0);

            message.PutShort(subPacketSixthId);
            message.PutShort(subPacketSixthSize);
            message.PutInt(count1);
            message.PutInt(count2);
            message.PutInt(count3);
            message.PutInt(count4);

            return MessageManager.Build(message, true);
        }
    }
}
