using System;
using System.Net;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Ava1.lib.core.io
{
    public enum PacketElementType : byte
    {
        None = 0,
        Byte = 1,
        Short = 2,
        Int = 3,
        Long = 4,
        Float = 5,
        String = 6,
        Bin = 7,
    }

    /// <summary>
    /// An exception for when a value of the wrong type is read from Packet.
    /// </summary>
    public class PacketElementTypeException : Exception
    {
        public PacketElementTypeException(PacketElementType expected, PacketElementType actual)
            : base(string.Format("Expected {0}, got {1}.", expected, actual))
        {
        }
    }

    /// <summary>
    /// General packet, used by Login and World.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Default size for the buffer
        /// </summary>
        private const int DefaultSize = 1024 * 2;

        /// <summary>
        /// Size added, every time the buffer runs out of space
        /// </summary>
        private const int AddSize = 1024;

        public byte[] _buffer;
        public int _ptr;
        protected int _bodyStart;
        private int _elements, _bodyLen;

        /// <summary>
        /// Packet's op code
        /// </summary>
        public ushort Op { get; set; }
        public ushort Lenght { get; set; }
        public int Size { get; set; }

        public int BytesAvaible
        {
            get
            {
                return (int)(this.Size - this._ptr);
            }
        }

        public Message(ushort op)
        {
            this.Op = op;

            _buffer = new byte[DefaultSize];
        }

        public Message(byte[] buffer, int offset)
        {
            _buffer = buffer;
            _ptr = offset;

            var length = buffer.Length;

            this.Size = buffer.Length;

            this.Op = BitConverter.ToUInt16(_buffer, _ptr);

            _ptr -= 2;

            this.Lenght = BitConverter.ToUInt16(_buffer, _ptr);

            _ptr += 5; 

            _bodyStart = _ptr;
         }

        /// <summary>
        /// Resets packet to zero while setting a new op and id,
        /// without allocating a new buffer.
        /// </summary>
        /// <param name="op"></param>
        /// <param name="id"></param>
        public void Clear(ushort op)
        {
            this.Op = op;

            Array.Clear(_buffer, 0, _buffer.Length);
            _ptr = 0;
            _bodyStart = 0;
            _elements = 0;
            _bodyLen = 0;
        }

        /// <summary>
        /// Returns the next element's type.
        /// </summary>
        /// <returns></returns>
        public PacketElementType Peek()
        {
            if (_ptr + 2 > _buffer.Length)
                return PacketElementType.None;
            return (PacketElementType)_buffer[_ptr];
        }

        /// <summary>
        /// Returns true if the next element to be read is of type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool NextIs(PacketElementType type)
        {
            return (this.Peek() == type);
        }

        /// <summary>
        /// Returns new empty packet.
        /// </summary>
        /// <returns></returns>
        public static Message Empty()
        {
            return new Message(0);
        }

        // Write
        // ------------------------------------------------------------------

        /// <summary>
        /// Adds one byte for type and the bytes in val to buffer.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        protected Message PutSimple(PacketElementType type, params byte[] val)
        {
            var len = 1 + val.Length;
            this.EnsureSize(len);

            Buffer.BlockCopy(val, 0, _buffer, _ptr, val.Length);
            _ptr += val.Length;
            _elements++;
            _bodyLen += len;

            return this;
        }

        /// <summary>
        /// Adds one byte for type, 2 bytes for the length of the val bytes,
        /// and the vals itself to buffer.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        protected Message PutWithLength(PacketElementType type, byte[] val)
        {
            var len = 1 + sizeof(short) + val.Length;
            this.EnsureSize(len);

            Buffer.BlockCopy(val, 0, _buffer, _ptr, val.Length);
            _ptr += val.Length - 1;

            _elements++;
            _bodyLen += len;

            return this;
        }

        /// <summary>Writes val to buffer.</summary>
        public Message PutByte(byte val) { return this.PutSimple(PacketElementType.Byte, val); }

        /// <summary>Writes val as byte to buffer.</summary>
        public Message PutByte(bool val) { return this.PutByte(val ? (byte)1 : (byte)0); }

        /// <summary>Writes val to buffer.</summary>
        public Message PutShort(short val) { return this.PutSimple(PacketElementType.Short, BitConverter.GetBytes(val)); }

        /// <summary>Writes val to buffer.</summary>
        public Message PutUShort(ushort val) { return this.PutShort((short)val); }

        /// <summary>Writes val to buffer.</summary>
        public Message PutInt(int val) { return this.PutSimple(PacketElementType.Int, BitConverter.GetBytes(val)); }

        /// <summary>Writes val to buffer.</summary>
        public Message PutUInt(uint val) { return this.PutInt((int)val); }

        /// <summary>Writes val to buffer.</summary>
        public Message PutLong(long val) { return this.PutSimple(PacketElementType.Long, BitConverter.GetBytes(val)); }

        /// <summary>Writes val to buffer.</summary>
        public Message PutULong(ulong val) { return this.PutLong((long)val); }

        /// <summary>Writes val as long to buffer.</summary>
        public Message PutLong(DateTime val) { return this.PutLong((long)(val.Ticks / 10000)); }

        /// <summary>Writes val to buffer.</summary>
        public Message PutFloat(float val) { return this.PutSimple(PacketElementType.Float, BitConverter.GetBytes(val)); }

        /// <summary>Writes val to buffer.</summary>
        public Message PutFloat(double val) { return this.PutFloat((float)val); }

        /// <summary>Writes val as null-terminated UTF8 string to buffer.</summary>
        public Message PutString(string val) { return this.PutWithLength(PacketElementType.String, Encoding.UTF8.GetBytes(val + "\0")); }

        /// <summary>Writes val as null-terminated UTF8 string to buffer.</summary>
        public Message PutString(string format, params object[] args) { return this.PutString(string.Format((format != null ? format : string.Empty), args)); }

        /// <summary>Writes val to buffer.</summary>
        public Message PutBin(byte[] val) { return this.PutWithLength(PacketElementType.Bin, val); }

        /// <summary>Writes bin containing a single 0 to buffer.</summary>
        public Message PutBin() { return this.PutBin(new byte[] { 0 }); }

        /// <summary>Converts struct to byte array and writes it as byte array to buffer.</summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when val is not a struct.
        /// </exception>
        public Message PutBin(object val)
        {
            var type = val.GetType();
            if (!type.IsValueType || type.IsPrimitive)
                throw new InvalidOperationException("PutBin only takes byte[] and structs.");

            var size = Marshal.SizeOf(val);
            var arr = new byte[size];

            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(val, ptr, true);
                Marshal.Copy(ptr, arr, 0, size);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr);
            }

            return this.PutBin(arr);
        }

        /// <summary>Writes packet as bin and the length of it as int to buffer.</summary>
        public Message PutBin(Message packet)
        {
            var val = packet.Build();
            return this.PutInt(val.Length).PutBin(val);
        }

        /// <summary>
        /// Resizes buffer, if there's not enough space for the required
        /// amount of bytes.
        /// </summary>
        /// <param name="required"></param>
        protected void EnsureSize(int required)
        {
            if (_ptr + required >= _buffer.Length)
                Array.Resize(ref _buffer, _buffer.Length + Math.Max(AddSize, required * 2));
        }

        // Read
        // ------------------------------------------------------------------

        /// <summary>
        /// Reads and returns byte from buffer.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PacketElementTypeException">
        /// Thrown if the type of the current value is not Byte.
        /// </exception>
        public byte GetByte()
        {
            _ptr += 1;
            return _buffer[(_ptr - 1)];
        }

        /// <summary>
        /// Reads and returns bool (byte) from buffer.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PacketElementTypeException">
        /// Thrown if the type of the current value is not Byte.
        /// </exception>
        public bool GetBool() { return (this.GetByte() != 0); }

        /// <summary>
        /// Reads and returns short from buffer.
        /// </summary>
        /// <returns></returns>
        public short GetShort()
        {
           /* if (this.Peek() != PacketElementType.Short)
                throw new PacketElementTypeException(PacketElementType.Short, this.Peek());

            this.AssertReadable(1 + sizeof(short));

            _ptr += 1;*/
            var val = BitConverter.ToInt16(_buffer, _ptr);

            _ptr += sizeof(short);

            return val;
        }

        /// <summary>
        /// Reads and returns ushort from buffer.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PacketElementTypeException">
        /// Thrown if the type of the current value is not Short.
        /// </exception>
        public ushort GetUShort()
        {
            return (ushort)this.GetShort();
        }

        /// <summary>
        /// Reads and returns int from buffer.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PacketElementTypeException">
        /// Thrown if the type of the current value is not Int.
        /// </exception>
        public int GetInt()
        {
           /* if (this.Peek() != PacketElementType.Int)
                throw new PacketElementTypeException(PacketElementType.Int, this.Peek());

            this.AssertReadable(1 + sizeof(int));*/

           // _ptr += 1;
            var val = BitConverter.ToInt32(_buffer, _ptr);
            _ptr += sizeof(int);

            return val;
        }

        /// <summary>
        /// Reads and returns uint from buffer.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PacketElementTypeException">
        /// Thrown if the type of the current value is not Int.
        /// </exception>
        public uint GetUInt()
        {
            return (uint)this.GetInt();
        }

        /// <summary>
        /// Reads and returns long from buffer.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PacketElementTypeException">
        /// Thrown if the type of the current value is not Long.
        /// </exception>
        public long GetLong()
        {
            if (this.Peek() != PacketElementType.Long)
                throw new PacketElementTypeException(PacketElementType.Long, this.Peek());

            this.AssertReadable(1 + sizeof(long));

            _ptr += 1;
            var val = IPAddress.HostToNetworkOrder(BitConverter.ToInt64(_buffer, _ptr));
            _ptr += sizeof(long);

            return val;
        }

        /// <summary>
        /// Reads and returns ulong from buffer.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PacketElementTypeException">
        /// Thrown if the type of the current value is not Long.
        /// </exception>
        public ulong GetULong()
        {
            return (ulong)this.GetLong();
        }

        /// <summary>
        /// Reads long from buffer and returns it as DateTime.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PacketElementTypeException">
        /// Thrown if the type of the current value is not Long.
        /// </exception>
        public DateTime GetDateTime()
        {
            return new DateTime(this.GetLong() * 10000);
        }

        /// <summary>
        /// Reads and returns float from buffer.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PacketElementTypeException">
        /// Thrown if the type of the current value is not Float.
        /// </exception>
        public float GetFloat()
        {
            if (this.Peek() != PacketElementType.Float)
                throw new PacketElementTypeException(PacketElementType.Float, this.Peek());

            this.AssertReadable(1 + sizeof(float));

            _ptr += 1;
            var val = BitConverter.ToSingle(_buffer, _ptr);
            _ptr += sizeof(float);

            return val;
        }

        /// <summary>
        /// Reads and returns string from buffer.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PacketElementTypeException">
        /// Thrown if the type of the current value is not String.
        /// </exception>
        public string GetString(int len)
        {
            var val = Encoding.UTF8.GetString(_buffer, _ptr, len);
            _ptr += len;

            return val;
        }

        /// <summary>
        /// Reads and returns bin from buffer.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PacketElementTypeException">
        /// Thrown if the type of the current value is not Bin.
        /// </exception>
        public byte[] GetBin()
        {
            if (this.Peek() != PacketElementType.Bin)
                throw new PacketElementTypeException(PacketElementType.Bin, this.Peek());

            this.AssertReadable(1 + sizeof(short));

            _ptr += 1;
            var len = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(_buffer, _ptr));
            _ptr += sizeof(short);

            this.AssertReadable(len);

            var val = new byte[len];
            Buffer.BlockCopy(_buffer, _ptr, val, 0, len);
            _ptr += len;

            return val;
        }

        /// <summary>
        /// Reads bin from buffer and returns it, marshaled into T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when type is not a struct.
        /// </exception>
        public T GetObj<T>() where T : new()
        {
            var type = typeof(T);
            if (!type.IsValueType || type.IsPrimitive)
                throw new InvalidOperationException("GetObj can only marshal to structs.");

            var buffer = this.GetBin();
            object val;

            IntPtr intPtr = IntPtr.Zero;
            try
            {
                intPtr = Marshal.AllocHGlobal(buffer.Length);
                Marshal.Copy(buffer, 0, intPtr, buffer.Length);
                val = Marshal.PtrToStructure(intPtr, typeof(T));
            }
            finally
            {
                if (intPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(intPtr);
            }

            return (T)val;
        }

        /// <summary>
        /// Throws IndexOutOfRangeException, if buffer doesn't have the given
        /// amount of bytes left.
        /// </summary>
        /// <param name="byteCount"></param>
        private void AssertReadable(int byteCount)
        {
            if (_ptr + byteCount > _buffer.Length)
                throw new IndexOutOfRangeException("Buffer doesn't have enough bytes left.");
        }

        /// <summary>
        /// Skips number of elements.
        /// </summary>
        /// <param name="num"></param>
        public void Skip(int num)
        {
            _ptr += num;
        }

        /// <summary>
        /// Get back number of elements.
        /// </summary>
        /// <param name="num"></param>
        public void Return(int num)
        {
            _ptr -= num;
        }

        // ------------------------------------------------------------------

        /// <summary>
        /// Reads variable lengthed number from buffer.
        /// </summary>
        /// <remarks>
        /// Used for the counters after op and id.
        /// http://en.wikipedia.org/wiki/LEB128
        /// </remarks>
        /// <param name="buffer"></param>
        /// <param name="ptr"></param>
        /// <returns></returns>
        private int ReadVarInt(byte[] buffer, ref int ptr)
        {
            int result = 0;

            for (int i = 0; ; ++i)
            {
                result |= (buffer[ptr] & 0x7f) << (i * 7);

                if ((buffer[ptr++] & 0x80) == 0)
                    break;
            }

            return result;
        }

        /// <summary>
        /// Writes variable lengthed number to buffer.
        /// </summary>
        /// <remarks>
        /// Used for the counters after op and id.
        /// http://en.wikipedia.org/wiki/LEB128
        /// </remarks>
        /// <param name="value"></param>
        /// <param name="buffer"></param>
        /// <param name="ptr"></param>
        private void WriteVarInt(int value, byte[] buffer, ref int ptr)
        {
            do
            {
                buffer[ptr++] = (byte)(value > 0x7F ? (0x80 | (value & 0xFF)) : value & 0xFF);
            }
            while ((value >>= 7) != 0);
        }

        // ------------------------------------------------------------------

        /// <summary>
        /// Returns size of the whole packet, incl header.
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            var i = 2;//4 + 8; // op + id + body

            int n = _bodyLen; // + body len
            do { i++; n >>= 7; } while (n != 0);

            n = _elements; // + number of elements
            do { i++; n >>= 7; } while (n != 0);

            ++i; // + zero
            i += _bodyLen; // + body

            return i;
        }

        /// <summary>
        /// Returns complete packet as byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] Build()
        {
            var result = new byte[this.GetSize()];
            this.Build(ref result, true, 0);

            return result;
        }

        /// <summary>
        /// Returns complete packet as byte array.
        /// </summary>
        /// <returns></returns>
        public void Build(ref byte[] buffer, bool startIndex, int offset)
        {
            //if (buffer.Length < offset + this.GetSize())
                //throw new Exception("Buffer too small for packet, use GetSize().");

            var length = _bodyLen;

            {
                // Calc checksum
                byte[] len = BitConverter.GetBytes(buffer.Length);
                byte[] head = BitConverter.GetBytes(this.Op);
                byte[] hash = BitConverter.GetBytes((UInt32)(Convert.ToDouble(len[0]) + Convert.ToDouble(len[1]) + Convert.ToDouble(head[0]) + Convert.ToDouble(head[1])));

                // Copy to
                Buffer.BlockCopy(head, 0, buffer, offset, sizeof(short));
                Buffer.BlockCopy(hash, 0, buffer, offset + 2, sizeof(short));

               // Buffer.BlockCopy(BitConverter.GetBytes(_bodyLen), 0, buffer, offset - 2, sizeof(byte));


                if (startIndex)
                    offset += 3;
                else
                    offset += 4;

                length += offset;
            }

            // Body
            Buffer.BlockCopy(_buffer, _bodyStart, buffer, offset, _bodyLen);        }

        /// <summary>
        /// Returns true if type is a valid value of the enum and not None.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsValidType(PacketElementType type)
        {
            return (type >= PacketElementType.Byte && type <= PacketElementType.Bin);
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            var prevPtr = _ptr;
            _ptr = _bodyStart;

            //result.AppendFormat("Op: {0:X08} {2}, Id: {1:X016}" + Environment.NewLine, this.Op, this.Id, Network.Op.GetName(this.Op));

            PacketElementType type;
            for (int i = 1; (this.IsValidType(type = this.Peek()) && _ptr < _buffer.Length); ++i)
            {
                if (type == PacketElementType.Byte)
                {
                    var data = this.GetByte();
                    result.AppendFormat("{0:000} [{1}] Byte   : {2}", i, data.ToString("X2").PadLeft(16, '.'), data);
                }
                else if (type == PacketElementType.Short)
                {
                    var data = this.GetShort();
                    result.AppendFormat("{0:000} [{1}] Short  : {2}", i, data.ToString("X4").PadLeft(16, '.'), data);
                }
                else if (type == PacketElementType.Int)
                {
                    var data = this.GetInt();
                    result.AppendFormat("{0:000} [{1}] Int    : {2}", i, data.ToString("X8").PadLeft(16, '.'), data);
                }
                else if (type == PacketElementType.Long)
                {
                    var data = this.GetLong();
                    result.AppendFormat("{0:000} [{1}] Long   : {2}", i, data.ToString("X16"), data);
                }
                else if (type == PacketElementType.Float)
                {
                    var data = this.GetFloat();

                    var hex = (BitConverter.DoubleToInt64Bits(data) >> 32).ToString("X8");
                    if (hex.Length > 8)
                        hex = hex.Substring(8);

                    result.AppendFormat("{0:000} [{1}] Float  : {2}", i, hex.PadLeft(16, '.'), data.ToString("0.0####", CultureInfo.InvariantCulture));
                }
                else if (type == PacketElementType.String)
                {
                   // var data = this.GetString();
                    //result.AppendFormat("{0:000} [................] String : {1}", i, data);
                }
                else if (type == PacketElementType.Bin)
                {
                    var data = BitConverter.ToString(this.GetBin());
                    var splitted = data.Split('-');

                    result.AppendFormat("{0:000} [................] Bin    : ", i);
                    for (var j = 1; j <= splitted.Length; ++j)
                    {
                        result.Append(splitted[j - 1]);
                        if (j < splitted.Length)
                            if (j % 16 == 0)
                                result.Append(Environment.NewLine.PadRight(34, ' '));
                            else
                                result.Append(' ');
                    }
                }

                result.AppendLine();
            }

            _ptr = prevPtr;

            return result.ToString();
        }
    }
}
