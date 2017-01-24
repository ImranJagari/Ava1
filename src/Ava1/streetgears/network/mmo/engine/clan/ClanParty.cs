using System;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

namespace Ava1.streetgears.network.mmo.engine.clan
{
    public class ClanParty
    {
        public Int32 Id;
        public String clanName;
        public Int32 clanExp;
        public Int32 clanLevel;
        public List<Client> Client;

        public static ClanParty getClan(string name)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM sg_clan WHERE clan_name = '" + name + "'", shared.database.Manager.MySqlConnection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }
                ClanParty _clanParty = new ClanParty((Int32)reader["id"], (String)reader["clan_name"], (Int32)reader["clan_exp"], (Int32)reader["clan_level"]);

                return _clanParty;
            }
        }

        public void SaveChanges()
        {
            new MySqlCommand($"UPDATE sg_clan SET clan_name = '{this.clanName}', SET '{this.clanExp}', SET '{this.clanLevel}' WHERE id = '{this.Id}' ", shared.database.Manager.MySqlConnection).ExecuteNonQuery();
        }

        public ClanParty(int _id, string _clanName, int _clanExp, int _clanLevel)
        {
            Id = _id;
            clanName = _clanName;
            clanExp = _clanExp;
            clanLevel = _clanLevel;
            Client = new List<Client>();
        }

        public void AddClientToParty(Client client)
        {
            Client.Add(client);
        }

        public void RemoveClientFromParty(Client client)
        {
            Client.Remove(client);
        }
    }
}
