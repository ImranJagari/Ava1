using System;

namespace Ava1.lib.core.io
{
    public class AuthAttribute : Attribute
    {
        public ushort Op { get; set; }

        public AuthAttribute(ushort op)
        {
            Op = op;
        }
    }
    
    public class MMOAttribute : Attribute
    {
        public ushort Op { get; set; }

        public MMOAttribute(ushort op)
        {
            Op = op;
        }
    }

    public class RelayAttribute : Attribute
    {
        public ushort Op { get; set; }

        public RelayAttribute(ushort op)
        {
            Op = op;
        }
    }
}
