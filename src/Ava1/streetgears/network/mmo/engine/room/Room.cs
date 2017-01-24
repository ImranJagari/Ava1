using System;

namespace Ava1.streetgears.network.mmo.engine.room
{
    public class Room
    {
        public Int32 Id;
        public String Name;
        public String Password;
        public Int32 Mode;
        public SByte currentPlayers;
        public SByte maxPlayers;
        public SByte State; // 0 = Locked 1 = Open
        public SByte License;

        public Room(int _Id, string _Name, string _Password, int _Mode, sbyte _currentPlayers, sbyte _maxPlayers, sbyte _State, sbyte _License)
        {
            Id = _Id;
            Name = _Name;
            Password = _Password;
            Mode = _Mode;
            currentPlayers = _currentPlayers;
            maxPlayers = _maxPlayers;
            State = _State;
            License = _License;
        }
    }
}
