using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.packet.mmo
{
    public class BM_SC_MATE_INFO : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static String successCmd = "SUCCESS\0";
        public static Int32 unk0;
        public static Int32 unk1;
        public static Int32 unk2;
        public static Int32 unk3;
        public static Int32 unk4;
        public static Int32 unk5;
        public static SByte unk6;
        public static SByte charType;
        public static String charName; // 43
        public static Int32 unk7;
        public static Int32 unk8;
        public static Int32 unk9;
        public static Int32 unk10;
        public static Int32 unk11;
        public static Int32 unk12;
        public static Int32 unk13;
        public static Int32 unk14;
        public static Int32 unk15;
        public static Int32 unk16;
        public static Int32 unk17;
        public static Int32 unk18;
        public static Int32 unk19;
        public static Int32 unk20;
        public static Int32 unk21;
        public static Int32 unk22;
        public static Int32 unk23;
        public static Int32 unk24;
        public static Int16 unk25;
        public static String clanTag; // ? 4
        public static String clanName; // 20
        public static Int32 unk26;
        public static Int32 unk27;
        public static Int32 unk28;
        public static Int32 unk29;
        public static Int32 unk30;
        public static Int32 unk31;
        public static Int32 unk32;
        public static Int32 unk33;
        public static Int32 unk34;
        public static Int32 unk35;
        public static Int32 unk36;
        public static Int32 unk37;
        public static Int32 unk38;
        public static Int32 unk39;
        public static Int32 unk40;
        public static Int32 unk41;
        public static Int16 unk42;
        public static SByte unk43;
        public static SByte charAge;
        public static Int16 charLevel;
        public static Int16 charLicense;
        public static Int16 charCountry; // mate_zip.txt
        public static Int16 unk44;
        public static String charZoneInfo; // 20
        public static Int32 unk45;
        public static Int32 unk46;
        public static Int32 unk47;
        public static Int32 unk48;
        public static Int32 unk49;
        public static Int32 unk50;
        public static Int32 unk51;
        public static Int32 unk52;
        public static Int32 unk53;
        public static Int32 unk54;
        public static Int32 unk55;
        public static Int32 unk56;
        public static Int32 unk57;
        public static Int32 unk58;
        public static Int32 unk59;
        public static Int32 unk60;
        public static Int32 unk61;
        public static Int32 unk62;
        public static Int32 unk63;
        public static Int32 unk64;
        public static Int32 unk65;
        public static Int32 unk66;
        public static Int32 unk67;
        public static Int32 unk68;
        public static Int32 unk69;
        public static SByte unk70;
        public static String charBio;

        public BM_SC_MATE_INFO(sbyte _charType, string _charName, string _clanTag, string _clanName, sbyte _charAge, short _charLevel, short _charLicense, short _charCountry, string _charZoneInfo, string _charBio)
        {
            charType = _charType;
            charName = _charName;
            clanTag = _clanTag;
            clanName = _clanName;
            charAge = _charAge;
            charLevel = _charLevel;
            charLicense = _charLicense;
            charCountry = _charCountry;
            charZoneInfo = _charZoneInfo;
            charBio = _charBio;
        }

        public override byte[] Pack()
        {
            var message = new Message(MmoServerEnums.BM_SC_MATE_INFO + 1);

            message.PutString(successCmd);
            message.PutInt(unk0);
            message.PutInt(unk1);
            message.PutInt(unk2);
            message.PutInt(unk3);
            message.PutInt(unk4);
            message.PutInt(unk5);
            message.PutByte((byte)unk6);
            message.PutByte((byte)charType);
            message.PutString(charName.PadRight(43, '\0'));
            message.PutInt(unk7);
            message.PutInt(unk8);
            message.PutInt(unk9);
            message.PutInt(unk10);
            message.PutInt(unk11);
            message.PutInt(unk12);
            message.PutInt(unk13);
            message.PutInt(unk14);
            message.PutInt(unk15);
            message.PutInt(unk16);
            message.PutInt(unk17);
            message.PutInt(unk18);
            message.PutInt(unk19);
            message.PutInt(unk20);
            message.PutInt(unk21);
            message.PutInt(unk22);
            message.PutInt(unk23);
            message.PutInt(unk24);
            message.PutShort(unk25);
            message.PutString(clanTag.PadRight(4, '\0'));
            message.PutString(clanName.PadRight(20, '\0'));
            message.PutInt(unk26);
            message.PutInt(unk27);
            message.PutInt(unk28);
            message.PutInt(unk29);
            message.PutInt(unk30);
            message.PutInt(unk31);
            message.PutInt(unk32);
            message.PutInt(unk33);
            message.PutInt(unk34);
            message.PutInt(unk35);
            message.PutInt(unk36);
            message.PutInt(unk37);
            message.PutInt(unk38);
            message.PutInt(unk39);
            message.PutInt(unk40);
            message.PutInt(unk41);
            message.PutShort(unk42);
            message.PutByte((byte)unk43);
            message.PutByte((byte)charAge);
            message.PutShort(charLevel);
            message.PutShort(charLicense);
            message.PutShort(charCountry);
            message.PutShort(unk44);
            message.PutString(charZoneInfo.PadRight(20, '\0'));
            message.PutInt(unk45);
            message.PutInt(unk46);
            message.PutInt(unk47);
            message.PutInt(unk48);
            message.PutInt(unk49);
            message.PutInt(unk50);
            message.PutInt(unk51);
            message.PutInt(unk52);
            message.PutInt(unk53);
            message.PutInt(unk54);
            message.PutInt(unk55);
            message.PutInt(unk56);
            message.PutInt(unk57);
            message.PutInt(unk58);
            message.PutInt(unk59);
            message.PutInt(unk60);
            message.PutInt(unk61);
            message.PutInt(unk62);
            message.PutInt(unk63);
            message.PutInt(unk64);
            message.PutInt(unk65);
            message.PutInt(unk66);
            message.PutInt(unk67);
            message.PutInt(unk68);
            message.PutInt(unk69);
            message.PutByte((byte)unk70);
            message.PutString(charBio.PadRight(100, '\0'));

            return MessageManager.Build(message, true);
        }
    }
}
