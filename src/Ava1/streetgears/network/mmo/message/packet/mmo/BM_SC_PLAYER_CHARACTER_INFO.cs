using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BM_SC_PLAYER_CHARACTER_INFO : PacketBase
    {
        public static Client client;

        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;
        public static String charName; 
        public static Int32 unk4 = 1;
        public static Int32 unk5 = 2;

        public static SByte unk6;
        public static SByte charType;
        public static SByte unk7;
        public static Int32 unk8;
        public static Int32 unk9;
        public static Int32 unk10;
        public static Int32 unk11;
        public static Int32 unk12;
        public static Int32 unk13;
        public static Int32 unk14;
        public static Int32 unk15;
        public static Int32 charLevel;
        public static Int32 unk16;
        public static Int32 unk17;
        public static Int32 unk18;
        public static Int32 unk19;
        public static Int32 unk20;

        public static Int32 head;
        public static Int32 face;
        public static Int32 upper;
        public static Int32 lower;
        public static Int32 foot;
        public static Int32 hand;
        public static Int32 google;
        public static Int32 accesoire;
        public static Int32 theme;
        public static Int32 mantle;
        public static Int32 buckle;
        public static Int32 vent;
        public static Int32 nitro;
        public static Int32 wheels;
        public static SByte unk21 = 37;
        public static Int32 unk22 = 38;
        public static Int32 trickSize = 13;

        public BM_SC_PLAYER_CHARACTER_INFO(Client _client, sbyte _charType, string _charName, int _charLevel)
        { 
            client = _client;
            charType = _charType;
            charName = _charName;
            charLevel = _charLevel;
        }

        public void PutTrick(Message message, int id, int level, byte apply)
        {
            message.PutInt(id);
            message.PutInt(level);
            message.PutByte(apply);
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_PLAYER_CHARACTER_INFO + 1);

            message.PutString(successCmd);
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutString(charName.PadRight(40, '\0'));
            message.PutInt(unk4);
            message.PutInt(unk5);
            message.PutByte((byte)unk6);
            message.PutByte((byte)charType);
            message.PutByte((byte)unk7);
            message.PutInt(unk8);
            message.PutInt(unk9);
            message.PutInt(unk10);
            message.PutInt(unk11);
            message.PutInt(unk12);
            message.PutInt(unk13);
            message.PutInt(unk14);
            message.PutInt(unk15);
            message.PutInt(charLevel);
            message.PutInt(unk16);
            message.PutInt(unk17);
            message.PutInt(unk18);
            message.PutInt(unk19);
            message.PutInt(unk20);
            message.PutInt(head);
            message.PutInt(face);
            message.PutInt(upper);
            message.PutInt(lower);
            message.PutInt(foot);
            message.PutInt(hand);
            message.PutInt(google);
            message.PutInt(accesoire);
            message.PutInt(theme);
            message.PutInt(mantle);
            message.PutInt(buckle);
            message.PutInt(vent);
            message.PutInt(nitro);
            message.PutInt(wheels);
            message.PutByte((byte)unk21);
            message.PutInt(unk22);
            message.PutInt(trickSize);

            for (int i = 0; i < (trickSize - 1); i++)
            {
                PutTrick(message, client.account.Trick[i].Id, client.account.Trick[i].Level, client.account.Trick[i].Apply);
            }

            return MessageManager.Build(message, true);
        }
    }
}
