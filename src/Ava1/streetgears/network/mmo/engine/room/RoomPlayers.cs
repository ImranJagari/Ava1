using System;

namespace Ava1.streetgears.network.mmo.engine.room
{
    public class RoomPlayers
    {
        public Int32 unk0;
        public Int32 unk1;
        public Int32 unk2;
        public Int32 unk3;
        public Int32 unk4;
        public Int32 unk5;
        public Int32 unk6;
        public Int32 unk7;
        public SByte unk8;
        public String charName;
        public Int16 unk9 = 16;
        public String charIp;
        public Int32 unk10 = 1;
        public Int16 unk11 = 1;
        public Int16 index;
        public SByte unk12 = 1;
        public SByte isMaster;

        public RoomPlayers(string _charName, string _charIp, short _index, sbyte _isMaster)
        {
            charName = _charName;
            charIp = _charIp;
            index = _index;
            isMaster = _isMaster;
        }
    }
}
