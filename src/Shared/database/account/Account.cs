using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Ava1.shared.database.account
{
    public class Account
    {
        public static Account getAccount(string username, string password)
        { 
            MySqlCommand command = new MySqlCommand("SELECT * FROM sg_account WHERE username = '" + username.Replace("\0", string.Empty) + "'" + " AND password = '" + password.Replace("\0", string.Empty) + "'", Manager.MySqlConnection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                Account _account = null;

                if (reader.Read())
                {
                    _account = new Account();

                    string[] trickInfo = reader["char_tricks"].ToString().Split('|', ',');
                    string[] inventoryInfo = reader["char_inventory"].ToString().Split('|', ',');

                    _account.char_id = (Int32)reader["id"];
                    _account.char_rank = (String)reader["char_rank"].ToString();
                    _account.first_login = (Int32)reader["first_login"];
                    _account.char_level = (Int32)reader["char_level"];
                    _account.char_type = (Int32)reader["char_type"];
                    _account.char_sex = (Int32)reader["char_sex"];
                    _account.char_age = (Int32)reader["char_age"];
                    _account.char_exp = (Int32)reader["char_exp"];
                    _account.char_licence = (Int32)reader["char_licence"];
                    _account.char_cash = (Int32)reader["char_cash"];
                    _account.char_gamecash = (Int32)reader["char_gamecash"];
                    _account.char_coin = (Int32)reader["char_coin"];
                    _account.char_questpoint = (Int32)reader["char_questpoint"];
                    _account.char_bio = (String)reader["char_bio"];
                    _account.char_zone = (String)reader["char_zone"];
                    _account.char_country = (Int32)reader["char_country"];
                    _account.char_clanname = (String)reader["char_clanname"];
                    _account.char_itemlist = (String)reader["char_inventory"];
                    _account.char_tricklist = (String)reader["char_tricks"];
                    _account.char_name = (String)reader["char_name"];
                    _account.char_rank = (String)reader["char_rank"];

                    _account.Trick.Add(new TrickList(Int32.Parse(trickInfo[0]), Int32.Parse(trickInfo[1]), Byte.Parse(trickInfo[2])));
                    _account.Trick.Add(new TrickList(Int32.Parse(trickInfo[3]), Int32.Parse(trickInfo[4]), Byte.Parse(trickInfo[5])));
                    _account.Trick.Add(new TrickList(Int32.Parse(trickInfo[6]), Int32.Parse(trickInfo[7]), Byte.Parse(trickInfo[8])));
                    _account.Trick.Add(new TrickList(Int32.Parse(trickInfo[9]), Int32.Parse(trickInfo[10]), Byte.Parse(trickInfo[11])));
                    _account.Trick.Add(new TrickList(Int32.Parse(trickInfo[12]), Int32.Parse(trickInfo[13]), Byte.Parse(trickInfo[14])));
                    _account.Trick.Add(new TrickList(Int32.Parse(trickInfo[15]), Int32.Parse(trickInfo[16]), Byte.Parse(trickInfo[17])));
                    _account.Trick.Add(new TrickList(Int32.Parse(trickInfo[18]), Int32.Parse(trickInfo[19]), Byte.Parse(trickInfo[20])));
                    _account.Trick.Add(new TrickList(Int32.Parse(trickInfo[21]), Int32.Parse(trickInfo[22]), Byte.Parse(trickInfo[23])));
                    _account.Trick.Add(new TrickList(Int32.Parse(trickInfo[24]), Int32.Parse(trickInfo[25]), Byte.Parse(trickInfo[26])));
                    _account.Trick.Add(new TrickList(Int32.Parse(trickInfo[27]), Int32.Parse(trickInfo[28]), Byte.Parse(trickInfo[29])));
                    _account.Trick.Add(new TrickList(Int32.Parse(trickInfo[30]), Int32.Parse(trickInfo[31]), Byte.Parse(trickInfo[32])));
                    _account.Trick.Add(new TrickList(Int32.Parse(trickInfo[33]), Int32.Parse(trickInfo[34]), Byte.Parse(trickInfo[35])));
                    _account.Trick.Add(new TrickList(Int32.Parse(trickInfo[36]), Int32.Parse(trickInfo[37]), Byte.Parse(trickInfo[38])));

                    if (_account.char_rank == "Admin" || _account.char_rank == "Developer")
                        _account.isAdmin = true;

                    for (int i = 0, j = 1, k = 2, l = 3, m = 0; i < inventoryInfo.Length && j < inventoryInfo.Length && k < inventoryInfo.Length && l < inventoryInfo.Length && m < inventoryInfo.Length; i += 4, j += 4, k += 4, l += 4, m++)
                        _account.Item.Add(new Inventory(m, Int32.Parse(inventoryInfo[i]), Int32.Parse(inventoryInfo[j]), Int32.Parse(inventoryInfo[k]), Int32.Parse(inventoryInfo[l])));
                }
                reader.Close();

                return _account;
            }
        }

        public void UpdateAccount()
        {
            if(this != null)
            {
                new MySqlCommand($"UPDATE sg_account SET char_rank = '{this.char_rank}', char_type = '{this.char_type}', char_sex = '{this.char_sex}', char_level = '{this.char_level}', char_exp = '{this.char_exp}', char_licence = '{this.char_licence}', char_cash = '{this.char_cash}', char_gamecash = '{this.char_gamecash}', char_coin = '{this.char_gamecash}', char_questpoint = '{this.char_questpoint}', char_tricks = '{this.char_tricklist}', char_inventory = '{this.char_itemlist}', char_clanname = '{this.char_clanname}', char_age = '{this.char_age}', char_zone = '{this.char_zone}', char_bio = '{this.char_bio}', char_country = '{this.char_country}', first_login = '{this.first_login}' WHERE id = '{this.char_id}' ", shared.database.Manager.MySqlConnection).ExecuteNonQuery();
            }
        }

        public String char_name { get; set; }
        public String char_rank { get; set; }
        public String char_clanname { get; set; }
        public String char_bio { get; set; }
        public String char_zone { get; set; }
        public Int32 char_country { get; set; }

        public Int32 char_age { get; set; }
        public Int32 char_id { get; set; }
        public Int32 first_login { get; set; }

        public Int32 char_type { get; set; }
        public Int32 char_sex { get; set; }
        public Int32 char_level { get; set; }
        public Int32 char_exp { get; set; }
        public Int32 char_licence { get; set; }
        public Int32 char_cash { get; set; }
        public Int32 char_gamecash { get; set; }
        public Int32 char_coin { get; set; }
        public Int32 char_questpoint { get; set; }

        public Boolean isInGame { get; set; }
        public Boolean isInLobby { get; set; }
        public Boolean isInLobbyRoom { get; set; }
        public Boolean isAdmin { get; set; }
        public Boolean isMotdSent { get; set; }

        public String char_itemlist { get; set; }
        public String char_tricklist { get; set; }

        public Boolean isReady { get; set; }
        public Boolean isLeader { get; set; }
        public Int32 roomId { get; set; }
        public Int32 roomSlot { get; set; }
        public SByte roomStatus { get; set; }
        public SByte roomEntry { get; set; }

        public List<Inventory> Item = new List<Inventory>();
        public List<TrickList> Trick = new List<TrickList>();
    }
}
