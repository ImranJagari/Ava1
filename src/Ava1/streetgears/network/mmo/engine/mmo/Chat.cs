using System;
using System.Linq;
using System.Collections.Generic;

using Ava1.streetgears.network.mmo.message.packet.mmo;
using Ava1.streetgears.network.mmo.message.packet.social;
using Ava1.lib.core.enums.network.global;
using Ava1.lib.core.utils;
using System.Text;

namespace Ava1.streetgears.network.mmo.engine.mmo
{
    public class Chat
    {
        public static Dictionary<int, int> LevelScale = new Dictionary<int, int>();

        public static void HandleCommands(Client client, string command)
        {
            var splittedCommand = command.Split(' ');
            switch(splittedCommand[0])
            {
                case ".help": HandleHelpMessage(client); break;
                case ".addcoins": HandleCoinsCommand(command); break;
                case ".addrupees": HandleRupeesCommand(command); break;
                case ".addgpotatos": HandleGpotatosCommand(command); break;
                case ".setlevel": HandleLevelCommand(command); break;
                case ".kick": HandleClientKicking(command); break;
                case ".rank": HandleRanking(command); break;
                case ".derank": HandleRanking(command); break;
                case ".ban": HandleRanking(command); break;
                case ".unban": HandleRanking(command); break;
                case ".clearinventory": HandleInventoryClearing(command); break;
                case ".removetricks": HandleTricksClearing(command); break;
                case ".addanimations": HandleAnimationsAdd(client); break;
                case ".announce": HandleAnnounce(command); break;
            }
        }

        public static void InitializeLevelScale()
        {
            LevelScale.Add(1, 150);
            LevelScale.Add(2, 350);
            LevelScale.Add(3, 550);
            LevelScale.Add(4, 800);
            LevelScale.Add(5, 1050);
            LevelScale.Add(6, 1350);
            LevelScale.Add(7, 1650);
            LevelScale.Add(8, 1950);
            LevelScale.Add(9, 2250);
            LevelScale.Add(10, 2700);
            LevelScale.Add(11, 3050);
            LevelScale.Add(12, 3400);
            LevelScale.Add(13, 3850);
            LevelScale.Add(14, 4300);
            LevelScale.Add(15, 4800);
            LevelScale.Add(16, 5300);
            LevelScale.Add(17, 5850);
            LevelScale.Add(18, 6400);
            LevelScale.Add(19, 6950);
            LevelScale.Add(20, 8055);
            LevelScale.Add(21, 9030);
            LevelScale.Add(22, 10005);
            LevelScale.Add(23, 10980);
            LevelScale.Add(24, 12215);
            LevelScale.Add(25, 13385);
            LevelScale.Add(26, 14620);
            LevelScale.Add(27, 15855);
            LevelScale.Add(28, 17155);
            LevelScale.Add(29, 18455);
            LevelScale.Add(30, 20080);
            LevelScale.Add(31, 21445);
            LevelScale.Add(32, 22810);
            LevelScale.Add(33, 24305);
            LevelScale.Add(34, 25800);
            LevelScale.Add(35, 27425);
            LevelScale.Add(36, 31325);
            LevelScale.Add(37, 36225);
            LevelScale.Add(38, 41125);
            LevelScale.Add(39, 47025);
            LevelScale.Add(40, 53925);
            LevelScale.Add(41, 61825);
            LevelScale.Add(42, 70725);
            LevelScale.Add(43, 79625);
            LevelScale.Add(44, 90960);
            LevelScale.Add(45, 10240);

            Log.WriteLog(Log.Type.Info, "Loaded '{0}' levels", LevelScale.Keys.Count);
        }

        public static void HandleHelpMessage(Client thisClient)
        {
            var helpMessage = string.Empty;
            if(thisClient.account.char_rank == "Admin" || thisClient.account.char_rank == "Developer")
            {
                helpMessage = "Commands:.help, .addcoins <coins> <target>, .addrupees <rupees> <target>, .addgpotato <gpo> <target>, .setlevel <level> <target>, .kick <target>, .rank <rank> <target>, .unrank <rank> <target>, .ban <target>, .unban <target>, .clearinventory <target>, .removetricks <target>, .addanimations <target>, .announce <message>";
            }
            thisClient.Send(new BM_SC_CHAT_MESSAGE(string.Empty, (sbyte)ChatTypeEnums.ChatType.CHAT_SYSTEM_INFO, (short)helpMessage.Length, helpMessage));
        }

        public static void HandleCoinsCommand(string command)
        {
            var splittedCommand = command.Split(' ');
            var thisClient = Server.clients.FirstOrDefault(x => x.account.char_name == splittedCommand[1]);
            var parsedValue = Int32.Parse(splittedCommand[2]);

            if (thisClient.account.char_coin < 999)
            {
                if (parsedValue < 999)
                {
                    if (thisClient.account.char_coin + parsedValue >= 999)
                    {
                         thisClient.account.char_coin = 999;
                    }
                    else
                    {
                         thisClient.account.char_coin += parsedValue;
                    }
                }
                else if (parsedValue >= 999)
                {
                    thisClient.account.char_coin = 999;
                }
                thisClient.Send(new BM_SC_REWARD_QUEST(0, 0, 0, parsedValue));
                thisClient.Send(new BM_SC_BALANCE_INFO(thisClient.account.char_gamecash, thisClient.account.char_coin, thisClient.account.char_cash, thisClient.account.char_questpoint));
            }
            thisClient.account.UpdateAccount();
        }

        public static void HandleRupeesCommand(string command)
        {
            var splittedCommand = command.Split(' ');
            var thisClient = Server.clients.FirstOrDefault(x => x.account.char_name == splittedCommand[1]);
            var parsedValue = Int32.Parse(splittedCommand[2]);

            if (thisClient.account.char_gamecash < int.MaxValue)
            {
                if (parsedValue < int.MaxValue)
                {
                    if (thisClient.account.char_gamecash + parsedValue >= int.MaxValue)
                    {
                        thisClient.account.char_gamecash = int.MaxValue;
                    }   
                    else
                    {
                        thisClient.account.char_gamecash += parsedValue;
                    }
                }
                else if (parsedValue >= int.MaxValue)
                {
                    thisClient.account.char_gamecash = int.MaxValue;
                }
                thisClient.Send(new BM_SC_REWARD_QUEST(0, 0, parsedValue, 0));
                thisClient.Send(new BM_SC_BALANCE_INFO(thisClient.account.char_gamecash, thisClient.account.char_coin, thisClient.account.char_cash, thisClient.account.char_questpoint));
            }
            thisClient.account.UpdateAccount();
        }

        public static void HandleGpotatosCommand(string command)
        {
            var splittedCommand = command.Split(' ');
            var thisClient = Server.clients.FirstOrDefault(x => x.account.char_name == splittedCommand[1]);
            var parsedValue = Int32.Parse(splittedCommand[2]);

            if (thisClient.account.char_cash < int.MaxValue)
            {
                if (parsedValue < int.MaxValue)
                {
                    if(thisClient.account.char_cash + parsedValue >= int.MaxValue)
                    {
                        thisClient.account.char_cash = int.MaxValue;
                    }
                    else
                    {
                        thisClient.account.char_cash += parsedValue;
                    }
                }
                else if (parsedValue >= int.MaxValue)
                {
                    thisClient.account.char_cash = int.MaxValue;
                }
                thisClient.Send(new BM_SC_CASH_BALANCE_INFO(thisClient.account.char_cash));
            }
            thisClient.account?.UpdateAccount();
        }

        public static void HandleLevelCommand(string command)
        {
            var splittedCommand = command.Split(' ');
            var thisClient = Server.clients.FirstOrDefault(x => x.account.char_name == splittedCommand[1]);
            var parsedValue = Int32.Parse(splittedCommand[2]);

            if (parsedValue < 45)
            {
                thisClient.Send(new BM_SC_LEVEL_INFO(LevelScale[parsedValue], parsedValue, thisClient.account.char_licence));
            }
            else if(parsedValue >= 45)
            {
                thisClient.Send(new BM_SC_LEVEL_INFO(LevelScale[45], 45, thisClient.account.char_licence));
            }
            thisClient.Send(new BM_SC_MMO_EVENT_MESSAGE(thisClient.account.char_exp));
        }

        public static void HandleClientKicking(string command)
        {
            var splittedCommand = command.Split(' ');
            var thisClient = Server.clients.FirstOrDefault(x => x.account.char_name == splittedCommand[1]);
            thisClient.Send(new BM_SC_CHAT_MESSAGE(string.Empty, (sbyte)ChatTypeEnums.ChatType.CHAT_DEBUG, 22, "Kicked from the server"));
            thisClient.Disconnect();
        }

        public static void HandleRanking(string command)
        {
            var splittedCommand = command.Split(' ');
            var thisClient = Server.clients.FirstOrDefault(x => x.account.char_name == splittedCommand[1]);
            var rankingMessage = string.Empty;

            switch (splittedCommand[0].ToString())
            {
                case "rank": rankingMessage = "Admin"; break;
                case "unrank": rankingMessage = "Normal"; break;
                case "ban": rankingMessage = "Banned"; break;
                case "unban": rankingMessage = "Normal"; break;
            }
            thisClient.account.char_rank = rankingMessage;
            thisClient.account?.UpdateAccount();
            thisClient.Send(new BM_SC_CHAT_MESSAGE(string.Empty, (sbyte)ChatTypeEnums.ChatType.CHAT_SYSTEM_ANNOUNCEMENT, (short)(26 + rankingMessage.Length), $"Your rank has changed to: {rankingMessage}!"));
            thisClient.Send(new BM_SC_MMO_EVENT_MESSAGE(thisClient.account.char_exp));
        }

        public static void HandleInventoryClearing(string command)
        {
            var splittedCommand = command.Split(' ');
            var thisClient = Server.clients.FirstOrDefault(x => x.account.char_name == splittedCommand[1]);
            thisClient.account.Item.Clear();
        }

        public static void HandleTricksClearing(string command)
        {
            var splittedCommand = command.Split(' ');
            var thisClient = Server.clients.FirstOrDefault(x => x.account.char_name == splittedCommand[1]);
            thisClient.account.Trick.Clear();
        }

        public static void HandleAnimationsAdd(Client thisClient)
        {
            var startId = int.MinValue;
            switch (thisClient.account.char_type)
            {
                case 0: startId = 100001; break;
                case 1: startId = 100101; break;
                case 2: startId = 100201; break;
                case 3: startId = 100301; break;
                case 4: startId = 100401; break;
                case 5: startId = 100501; break;
            }
            for (int i = startId, j = 1; i < (startId + 13) && j < 13; i++, j++)
            {
                thisClient.account.Item.Add(new shared.database.account.Inventory(thisClient.account.Item.Count() + j, i, 0, 10, 1));
            }
            thisClient.Send(new BM_SC_CHAT_MESSAGE(string.Empty, (sbyte)ChatTypeEnums.ChatType.CHAT_SYSTEM_ANNOUNCEMENT, (short)42, $"Added tricks animations to your inventory!"));
        }

        public static void HandleAnnounce(string command)
        {
            var splittedCommand = command.Split(' ');
            var splittedText = command.Split('.');
            string announceMessage = splittedText[1].ToString().Replace("announce ", string.Empty);
            Server.clients.ForEach(x => x.Send(new BM_SC_CHAT_MESSAGE(string.Empty, (sbyte)ChatTypeEnums.ChatType.CHAT_SYSTEM_ANNOUNCEMENT, (short)announceMessage.Length, announceMessage)));
        }
    }
}
