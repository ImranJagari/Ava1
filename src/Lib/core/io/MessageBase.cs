namespace Ava1.lib.core.io
{
    public abstract class PacketBase
    {
        public abstract ushort packetOp { get; set; }
        public abstract byte[] Pack();
    }
}
