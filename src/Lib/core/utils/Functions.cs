using System;

namespace Ava1.lib.core.utils
{
    public class Functions
    {
        private static Random random = new Random();

        public static string Random(int lenght, bool upper)
        {
            string str = string.Empty;
            for (int i = 1; i <= lenght; i++)
            {
                int num = random.Next(0, 26);
                str += (char)('a' + num);
            }
            if (upper)
                return str.ToUpper();
            return str;
        }

        public static string ByteArrayToString(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", " ");
        }
    }
}
