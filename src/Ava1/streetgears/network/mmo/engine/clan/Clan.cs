using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;
using Ava1.shared.database;
using Ava1.lib.core.utils;

namespace Ava1.streetgears.network.mmo.engine.clan
{
    public class Clan
    {
        public static List<Clan> thisClan = new List<Clan>();

        public Int32 id;
        public String name;
        public Int32 level;
        public Int32 exp;
        public String motd;
        public String members;
        public Int32 points;

        public Clan(int _id, string _name, int _level, int _exp, string _motd, string _members, int _points)
        {
            id = _id;
            name = _name;
            level = _level;
            exp = _exp;
            motd = _motd;
            members = _members;
            points = _points;
        }

        public static int getClanCount()
        {
            return thisClan.Count;
        }

        public static void Initialize()
        {
            using (MySqlDataReader reader = new MySqlCommand("SELECT * FROM sg_clan", Manager.MySqlConnection).ExecuteReader())
            {
                while (reader.Read())
                {
                    thisClan.Add(new Clan((Int32)reader["id"], (String)reader["clan_name"], (Int32)reader["clan_level"], (Int32)reader["clan_exp"], (String)reader["clan_motd"], (String)reader["clan_members"], (Int32)reader["clan_points"]));
                }
                reader.Close();
                Log.WriteLog(Log.Type.Info, "Loaded '{0}' clan", getClanCount());
            }
        }

        public static void AddClanToDatabase(int id, string clanName, int clanLevel, int clanExp, string clanMotd, string clanMembers, int clanPoints)
        {
            MySqlCommand cmd = new MySqlCommand($"INSERT INTO sg_clan (id, clan_name, clan_level, clan_exp, clan_motd, clan_members, clan_points) VALUES (@id, @clan_name, @clan_level, @clan_exp, @clan_motd, @clan_members, @clan_points)", Manager.MySqlConnection);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@clan_name", clanName.Replace("\0", string.Empty));
            cmd.Parameters.AddWithValue("@clan_level", clanLevel);
            cmd.Parameters.AddWithValue("@clan_exp", clanExp);
            cmd.Parameters.AddWithValue("@clan_motd", clanMotd);
            cmd.Parameters.AddWithValue("@clan_members", clanMembers);
            cmd.Parameters.AddWithValue("@clan_points", clanPoints);
            cmd.ExecuteNonQuery();
        }
    }
}
