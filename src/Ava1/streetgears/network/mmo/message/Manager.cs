using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Ava1.lib.core.io;
using Ava1.lib.core.utils;

namespace Ava1.streetgears.network.mmo.message
{
    public class Manager
    {
        public static Dictionary<ushort, MethodInfo> MethodHandlers = new Dictionary<ushort, MethodInfo>();

        public static void Initialize(Assembly asm)
        {
            var methods = asm.GetTypes().SelectMany(t => t.GetMethods()).Where(m => m.GetCustomAttributes(typeof(MMOAttribute), false).Length > 0).ToArray();
            foreach (var method in methods)
            {
                MethodHandlers.Add((ushort)method.CustomAttributes.ToArray()[0].ConstructorArguments[0].Value, method);
            }
            Log.WriteLog(Log.Type.Info, "Loaded '{0}' mmo methods", methods.Length);
        }

        public static List<Message> messageList = new List<Message>();
        public static bool bytes_avaible = false;
        public static byte[] buffer;
        
        public static void Handle(Client client, Message message)
        {
            MethodInfo methodToInvok;

            try
            {
                if (message != null)
                {
                    if (message.Lenght < message.Size)
                    {
                        var offset = 0;
                        try
                        {
                            while (offset < message.Size - 1)
                            {
                                cphl_0:
                                var lenght = message._buffer[offset] | message._buffer[offset + 1] << 8;
                                var header = message._buffer[offset + 2] | message._buffer[offset + 3] << 8;

                                if (lenght == header)
                                {
                                    offset += 2; goto cphl_0;
                                }
                                if (message._buffer[offset] == message._buffer[offset + 1])
                                {
                                    offset += 1; goto cphl_0;
                                }

                                if ((message.Size - offset) >= lenght)
                                {
                                    buffer = new byte[(int)lenght];
                                    Buffer.BlockCopy(message._buffer, offset, buffer, 0, lenght);
                                    messageList.Add(new Message(buffer, 2));
                                }

                                foreach (Message msg in messageList)
                                {
                                    if (MethodHandlers.TryGetValue((ushort)msg.Op, out methodToInvok))
                                    {
                                        methodToInvok.Invoke(null, new object[] { client, msg });
                                    }
                                }
                                messageList.Clear();
                                offset += lenght;
                            }
                        }
                        catch(Exception ex)
                        {
                        }
                    }
                    else
                    {
                        if (MethodHandlers.TryGetValue((ushort)message.Op, out methodToInvok))
                        {
                            methodToInvok.Invoke(null, new object[] { client, message });
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
