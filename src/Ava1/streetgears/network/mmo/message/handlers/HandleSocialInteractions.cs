using System;
using System.Linq;

using Ava1.lib.core.utils;
using Ava1.lib.core.io;
using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.enums.network.global;
using Ava1.streetgears.network.mmo.engine.mmo;
using Ava1.streetgears.network.mmo.engine.clan;
using Ava1.streetgears.network.mmo.message.packet.mmo;
using Ava1.streetgears.network.mmo.message.packet.social;

namespace Ava1.streetgears.network.mmo.message.handlers
{
    public class HandleSocialInteractions
    {
        [MMO((UInt16)MmoServerEnums.BM_SC_MATE_INFO)]
        public static void HandleMateInfo(Client client, Message message)
        {
            if (message.BytesAvaible >= 24)
            {
                var unk1 = message.GetInt();
                var charName = message.GetString(20);
                var clanTag = string.Empty;
                var mate = Server.clients.FirstOrDefault(x => x.account.char_name == charName.Replace("\0", string.Empty));
                if (mate != null)
                {
                    if (mate.account.char_clanname != null || mate.account.char_clanname != string.Empty)
                    {
                        clanTag = "CLAN";
                    }
                    else
                    {
                        clanTag = string.Empty;
                    }
                    client.Send(new BM_SC_MATE_INFO((sbyte)mate.account.char_type, mate.account.char_name, clanTag, mate.account.char_clanname, (sbyte)mate.account.char_age, (short)mate.account.char_level, (short)mate.account.char_licence, (short)mate.account.char_country, mate.account.char_zone, mate.account.char_bio));
                }
                else
                {
                    Log.WriteLog(Log.Type.Info, "{0} selected unknow player for mate info", client.account.char_name);
                }
            }
        }

        [MMO((UInt16)MmoServerEnums.BM_SC_UPDATE_MYMATEINFO)]
        public static void HandleUpdateMateInfo(Client client, Message message)
        {
            if (message.BytesAvaible >= 53)
            {
                var myAge = message.GetByte();
                var myZoneId = message.GetInt();
                var myZoneInfo = message.GetString(121);
                var myBioStr = message.GetString(151);
                var isPrivate = message.GetByte();
                var myGender = message.GetByte();

                client.account.char_age = myAge;
                client.account.char_country = myZoneId;
                client.account.char_zone = myZoneInfo.Replace("\0", string.Empty);
                client.account.char_bio = myBioStr.Replace("\0", string.Empty);
                client.account.char_sex = myGender;

                client.account?.UpdateAccount();

                client.Send(new BM_SC_UPDATE_MYMATEINFO());

                Log.WriteLog(Log.Type.Info, "'{0}' updated his info: Age={1}, ZoneId={2}, ZoneInfo={3}, Bio={4}, Gender={5}", client.account.char_name, client.account.char_age, client.account.char_country, client.account.char_zone, client.account.char_bio, client.account.char_sex);
            }
        }

        [MMO((UInt16)MmoServerEnums.BM_SC_CREATE_CLAN)]
        public static void HandleClanCreation(Client client, Message message)
        {
            if (message.BytesAvaible >= 20)
            {
                var clanName = message.GetString(20);

                var thisClan = Clan.thisClan.FirstOrDefault(x => x.name == clanName);
                if (thisClan == null)
                {
                    if (client.account.char_level >= 10)
                    {
                        if (client.account.char_gamecash >= 2000)
                        {
                            client.account.char_gamecash -= 2000;
                            client.account.char_clanname = clanName;

                            Clan.AddClanToDatabase((Clan.getClanCount()), clanName, 1, 0, "", $"{client.account.char_name},2|", 0); 

                            client.Send(new BM_SC_CREATE_CLAN(clanName));
                            client.Send(new BM_SC_CREATE_CLAN_RESULT("guild_create_success"));
                            client.Send(new BM_SC_BALANCE_INFO(client.account.char_gamecash, client.account.char_coin, client.account.char_cash, client.account.char_questpoint));

                            Log.WriteLog(Log.Type.Info, "'{0}' created clan '{1}'", client.account.char_name, clanName);
                        }
                        else
                        {
                            client.Send(new BM_SC_CREATE_CLAN_RESULT("B_NOT_ENOUGH_GAME_MONEY"));
                        }
                    }
                    else
                    {
                        client.Send(new BM_SC_CREATE_CLAN_RESULT("guild_create_level"));
                    }
                    client.account?.UpdateAccount();
                }
                else
                {
                    client.Send(new BM_SC_CREATE_CLAN_RESULT("guild_exist"));
                }
            }
        }

        [MMO((UInt16)MmoServerEnums.BM_SC_CHAT_USER)]
        public static void HandleChat(Client client, Message message)
        {
            if (message.BytesAvaible >= 37)
            {
                var msgSender = message.GetString(33);
                var msgType = message.GetByte(); 
                var msgLenght = message.GetShort();
                var msg = message.GetString(message.BytesAvaible);

                if (msg[0] == '.' && msgType == 0)
                {
                    if (client.account.char_rank == "Developer" || client.account.char_rank == "Admin")
                    {
                        Chat.HandleCommands(client, msg);
                    }
                }
                else
                {
                    var msgChatType = sbyte.MinValue;

                    switch(msgType)
                    {
                        case 0: msgChatType = (sbyte)ChatTypeEnums.ChatType.CHAT_NORMAL; break;
                        case 3: msgChatType = (sbyte)ChatTypeEnums.ChatType.CHAT_WHISP_OUT; break;
                        case 5: msgChatType = (sbyte)ChatTypeEnums.ChatType.CHAT_ALL; break; 
                    }

                    if(client.account.isInGame)
                    {
                        var chatType = sbyte.MinValue;
                        if (client.account.char_rank == "Admin" || client.account.char_rank == "Developper")
                        {
                            chatType = (sbyte)ChatTypeEnums.ChatType.CHAT_GM;
                        }
                        else
                        {
                            chatType = msgChatType;
                        }
                        foreach (Client _client in Server.clients)
                        {
                            if (_client.account.isInGame && _client != client)
                            {
                                _client.Send(new BM_SC_CHAT_MESSAGE(client.account.char_name, chatType, (short)msgLenght, msg));
                            }
                        }
                    }
                    else if(client.account.isInLobby)
                    {
                        var chatType = sbyte.MinValue;
                        if (client.account.char_rank == "Admin" || client.account.char_rank == "Developper")
                        {
                            chatType = (sbyte)ChatTypeEnums.ChatType.CHAT_GM;
                        }
                        else
                        {
                            chatType = msgChatType;
                        }
                        foreach (Client _client in Server.clients)
                        {
                            if (_client.account.isInLobby && _client != client)
                            {
                                _client.Send(new BM_SC_CHAT_MESSAGE(client.account.char_name, chatType, (short)msgLenght, msg));
                            }
                        }
                    }
                    else if(client.account.isInLobbyRoom)
                    {
                        var chatType = sbyte.MinValue;
                        if (client.account.char_rank == "Admin" || client.account.char_rank == "Developper")
                        {
                            chatType = (sbyte)ChatTypeEnums.ChatType.CHAT_GM;
                        }
                        else
                        {
                            chatType = msgChatType;
                        }
                        if (client.account.roomId != -1)
                        {
                            var thisRoom = HandleRoomInteractions.RoomParty.FirstOrDefault(x => x.Client.Contains(client));
                            if (thisRoom != null)
                            {
                                thisRoom.Send(new BM_SC_CHAT_MESSAGE(client.account.char_name, chatType, (short)msgLenght, msg));
                            }
                        }
                    }
                    client.Send(new BM_SC_CHAT_USER());
                    Log.WriteLog(Log.Type.Info, "{0}: '{1}'", client.account.char_name, msg);
                }
            }
        }

        [MMO((UInt16)MmoServerEnums.MM_SC_MSN)]
        public static void HandleMsnInitialization(Client client, Message message)
        {
            client.Send(new MM_SC_MSN());
            Log.WriteLog(Log.Type.Info, "Initialized MSN instance");
        }
    }
}
