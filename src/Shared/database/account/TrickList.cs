using System;

namespace Ava1.shared.database.account
{
    public class TrickList
    {
        public Int32 Id;
        public Int32 Level;
        public Byte Apply;

        public TrickList(int _Id, int _Level, byte _Apply)
        {
            Id = _Id;
            Level = _Level;
            Apply = _Apply;
        }
    }
}
