using System;

namespace Ava1.lib.core.utils
{
    public class Log
    {
        public static void Start()
        {
            Console.WindowWidth = Console.WindowWidth - 8;
            Console.WindowHeight = Console.WindowHeight - 8;

            Console.Title = "Ava1";
        }

        public enum Type
        {
            Info,
            Debug,
            Error,
            Recv,
            Send,
        };

        public static void WriteLog(Type t, string s, params object[] o)
        {
            switch (t)
            {
                case Type.Info:
                case Type.Debug:
                case Type.Error:
                case Type.Send:
                    Console.Write("[{0}] [{1}]:", DateTime.Now.ToString("h:mm:sstt"), t);
                    break; 
            }
            Console.SetCursorPosition(18, Console.CursorTop);
            Console.WriteLine(s, o);
        }
    }
}
