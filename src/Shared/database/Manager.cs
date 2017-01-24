using System;

using Ava1.lib.core.utils;
using MySql.Data.MySqlClient;
using Ava1.lib.core.config;

namespace Ava1.shared.database
{
    public class Manager
    {
        public static MySqlConnection MySqlConnection { get; set; }
        public static MySqlCommand Command { get; set; }

        public static Boolean isLoaded = false;

        public enum Databases
        {
            sg_account,
            sg_server,
            sg_clan,
        }

        public static void Connect()
        {
            try
            {
                MySqlConnection = new MySqlConnection();
                MySqlConnection.ConnectionString = string.Format("server={0};uid={1};pwd={2};database={3};", Config.elements["SQL"]["Host"], Config.elements["SQL"]["Username"], Config.elements["SQL"]["Password"], Config.elements["SQL"]["Database"]);
                MySqlConnection.Open();

                Log.WriteLog(Log.Type.Info, "Connected to MySQL database '{0}', Username: {1}, Password: {2}", MySqlConnection.Database, Config.elements["SQL"]["Username"], Config.elements["SQL"]["Password"]);

                foreach(var databases in Enum.GetValues(typeof(Databases)))
                {
                    if(CheckIfDatabaseExists(databases.ToString()))
                    {
                        isLoaded = true;
                    }
                }
                if(isLoaded)
                {
                    Log.WriteLog(Log.Type.Info, $"Loaded '{Enum.GetNames(typeof(Databases)).Length}' tables from {MySqlConnection.Database}");
                }
            }
            catch (MySqlException ex)
            {
                Log.WriteLog(Log.Type.Error, "{0}", ex.ToString());
            }
        }

        public static void Close()
        {
            try
            {
                MySqlConnection = null;
                MySqlConnection.Close();
            }
            catch (MySqlException ex)
            {
                Log.WriteLog(Log.Type.Error, ex.ToString());
            }
        }

        public static void ExecuteCommand(string cmd)
        {
            new MySqlCommand(cmd, MySqlConnection).ExecuteNonQuery();
        }

        public static bool CheckIfDatabaseExists(string databaseName)
        {        
            try
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM " + databaseName + " WHERE id = '0'", shared.database.Manager.MySqlConnection);
                return true;
            }
            catch(Exception ex)
            {
                Log.WriteLog(Log.Type.Error, ex.ToString());
                return false;
            }
        }

        public static bool SearchUserByName(string username)
        {
            using (MySqlDataReader reader = new MySqlCommand("SELECT * FROM sg_account", Manager.MySqlConnection).ExecuteReader())
            {
                while(reader.Read())
                {
                    if (username == (String)reader["char_name"])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                reader.Close();
                return false;
            }
        }
    }
}
