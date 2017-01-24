using System.IO;
using System.Collections.Generic;

using Ava1.lib.core.utils;

namespace Ava1.lib.core.config
{
    public class Config
    {
        public static Dictionary<string, Dictionary<string, string>> elements = new Dictionary<string, Dictionary<string, string>>();
        public static Dictionary<string, string> currentGroup = null;

        public static void ReadSettings(string Path)
        {
            elements.Clear();

            StreamReader reader = new StreamReader(Path);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line != "" && !line.StartsWith(";"))
                {
                    if (line.StartsWith("["))
                    {
                        currentGroup = new Dictionary<string, string>();
                        elements.Add(line.Replace("[", "").Replace("]", ""), currentGroup);
                    }
                    else if (currentGroup != null)
                    {
                        string[] data = line.Trim().Split('=');
                        string key = data[0].Trim();
                        string value = data[1].Trim();
                        currentGroup.Add(key, value);
                    }
                }
            }
            Log.WriteLog(Log.Type.Info, "Loaded '{0}' entries from Ava1.ini:", elements.Count);
            reader.Close();
        }
    }
}
