using System.IO;

namespace Ava1.lib.core.io
{
    public class MessageManager
    {
        public static void EncodeBuffer() { }

        public static void DecodeBuffer() { }

        public static void ClearBuffer() { }

        public static byte[] Build(Message _message, bool startIndex)
        {
            var result = new byte[_message.GetSize() + 6];
            using (BinaryWriter binWriter = new BinaryWriter(new MemoryStream(result)))
            {
                binWriter.Seek(0, SeekOrigin.Begin);
                binWriter.Write(_message.GetSize() + 6);
            }
            _message.Build(ref result, startIndex, 2);
            return result;
        }
    }
}
