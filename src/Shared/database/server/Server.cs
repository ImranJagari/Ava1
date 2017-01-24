using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

using Ava1.lib.core.utils;

namespace Ava1.shared.database.server
{
    public class Server
    {
        public static List<Server> Servers = new List<Server>();

        public Int32 id;
        public String name;
        public Int32 channel_status;
        public Int32 channel_currentnum;
        public Int32 channel_maxnum;

        public static int getServerCount()
        {
            return Servers.Count;
        }

        public Server(int _id, string _name, int _channel_status, int _channel_currentnum, int _channel_maxnum)
        {
            id = _id;
            name = _name;
            channel_status = _channel_status;
            channel_currentnum = _channel_currentnum;
            channel_maxnum = _channel_maxnum;
        }

        public static void Initialize()
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM sg_server", shared.database.Manager.MySqlConnection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Servers.Add(new Server((Int32)reader["id"], (String)reader["name"], (Int32)reader["channel_status"], (Int32)reader["channel_currentnum"], (Int32)reader["channel_maxnum"]));
                }
                Log.WriteLog(Log.Type.Info, "Loaded '{0}' StreetGears server", getServerCount());
            }
        }

        public void UpdateServer()
        {
            if (this != null)
            {
                new MySqlCommand($"UPDATE sg_server SET id = '{this.id}', name = '{this.name}', channel_status = '{this.channel_status}', channel_maxnum = '{this.channel_maxnum}', channel_currentnum = '{this.channel_currentnum}' WHERE id = '{this.channel_currentnum}'", Manager.MySqlConnection).ExecuteNonQuery();
            }
        }
    }
}
