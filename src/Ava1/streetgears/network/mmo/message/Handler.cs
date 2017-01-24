using System;
using System.Collections.Generic;
using System.Linq;

using Ava1.lib.core.net.enums;
using Ava1.lib.core.io;
using Ava1.lib.utils;
using Ava1.lib.config;
using Ava1.auth.core.net.mmo.message.packet;

namespace Ava1.auth.core.net.mmo.message
{
    public class Handler
    {
        [MMO((UInt16)MmoServerEnums.BM_SC_CHAT_USER)]
        public static void HandleChat(Client client, Message message)
        {
            if (message.BytesAvaible >= 39)
            {
                var unk0 = message.GetShort();
                var msgSender = message.GetString(33);
                var msgType = message.GetByte(); // 0 = normal chat; 3 = whisper; 5 = shout 
                var msgLenght = message.GetShort();
                var msg = message.GetString(message.BytesAvaible);

                var chatCmd = string.Empty;
                var parsedCmd = string.Empty;
                var parsedCmd2 = string.Empty;

                if (msg[0] == '.' && msgType == 0)
                {
                    Console.WriteLine($"Chat {msg}");

                    parsedCmd = msg.
                        Replace(" ", string.Empty).
                        Replace(".", string.Empty).
                        Replace("0", string.Empty).
                        Replace("1", string.Empty).
                        Replace("2", string.Empty).
                        Replace("3", string.Empty).
                        Replace("4", string.Empty).
                        Replace("5", string.Empty).
                        Replace("6", string.Empty).
                        Replace("7", string.Empty).
                        Replace("8", string.Empty).
                        Replace("9", string.Empty).
                        Replace("\0", string.Empty);

                    parsedCmd2 = msg.
                        Replace(" ", string.Empty).
                        Replace(".", string.Empty).
                        Replace("\0", string.Empty).
                        Replace("addcoins", string.Empty).
                        Replace("addrupees", string.Empty).
                        Replace("addgpotatos", string.Empty).
                        Replace("addanimations", string.Empty).
                        Replace("rank", string.Empty).
                        Replace("derank", string.Empty).
                        Replace("ban", string.Empty).
                        Replace("unban", string.Empty).
                        Replace("announce", string.Empty);

                    Console.WriteLine($"TESTETSETSe {parsedCmd} {parsedCmd2}");

                    if (client.account.char_rank == "Developer" || client.account.char_rank == "Admin")
                    {
                        if (parsedCmd == "disconnect")
                        {
                            client.Disconnect();
                        }
                        else if (parsedCmd.Contains("addcoins"))
                        {
                            engine.mmo.Chat.HandleCoinsCommand(client, parsedCmd, parsedCmd2);
                        }
                        else if (parsedCmd.Contains("addrupees"))
                        {
                            engine.mmo.Chat.HandleRupeesCommand(client, parsedCmd, parsedCmd2);
                        }
                        else if (parsedCmd.Contains("addgpotatos"))
                        {
                            engine.mmo.Chat.HandleGpotatosCommand(client, parsedCmd, parsedCmd2);
                        }
                        else if (parsedCmd.Contains("addexp"))
                        {
                            engine.mmo.Chat.HandleExpCommand(client, parsedCmd, parsedCmd2);
                        }
                        else if (parsedCmd.Contains("rank"))
                        {
                            if (client.account.char_rank == "Developper" || client.account.char_rank == "Admin")
                            {
                                var mate = Server.clients.FirstOrDefault(x => x.account.char_name == parsedCmd2);
                                if (mate != null)
                                {
                                    if (mate.account.char_rank != "Admin")
                                        mate.account.char_rank = "Admin";
                                }
                            }
                        }
                        else if (parsedCmd.Contains("derank"))
                        {
                            if (client.account.char_rank == "Developper")
                            {
                                var mate = Server.clients.FirstOrDefault(x => x.account.char_name == parsedCmd2);
                                if (mate != null)
                                {
                                    mate.account.char_rank = "Normal";
                                }
                            }
                        }
                        else if (parsedCmd.Contains("kick"))
                        {
                            var mate = Server.clients.FirstOrDefault(x => x.account.char_name == parsedCmd2);
                            if (mate != null)
                            {
                                mate.Disconnect();
                            }
                        }
                        else if (parsedCmd.Contains("ban"))
                        {
                            var mate = Server.clients.FirstOrDefault(x => x.account.char_name == parsedCmd2);
                            if (mate != null)
                            {
                                if (mate.account.char_rank != "Admin" || mate.account.char_rank != "Developper")
                                    mate.account.char_rank = "Banned";
                            }
                        }
                        else if (parsedCmd.Contains("unban"))
                        {
                            var mate = Server.clients.FirstOrDefault(x => x.account.char_name == parsedCmd2);
                            if (mate != null)
                            {
                                mate.account.char_rank = "Normal";
                            }
                        }
                        else if (parsedCmd == "addanimations")
                        {
                            engine.mmo.Chat.HandleAnimations(client, parsedCmd, parsedCmd2);
                        }
                        else if (parsedCmd.Contains("clearinventory"))
                        {
                            var mate = Server.clients.FirstOrDefault(x => x.account.char_name == parsedCmd2);
                            if (mate != null)
                            {
                                mate.account.Item.Clear();
                            }
                        }
                        else if (parsedCmd.Contains("removetricks"))
                        {
                            var mate = Server.clients.FirstOrDefault(x => x.account.char_name == parsedCmd2);
                            if (mate != null)
                            {
                                mate.account.Trick.ForEach(trick => trick.Apply = 0);
                            }
                        }
                        else if (parsedCmd.Contains("announce"))
                        {
                            foreach (Client _client in Server.clients)
                            {
                                _client.Send(new BM_SC_CHAT_MESSAGE(string.Empty, (sbyte)Ava1.lib.core.net.enums.packet.ChatTypeEnums.ChatType.CHAT_SYSTEM_ANNOUNCEMENT, (short)parsedCmd2.Length, parsedCmd2));
                            }
                        }
                    }
                }
                else
                {
                    //if (msgType == 0 || msgType == 5)
                    //{
                        //if (client.account.isInGame || client.account.isInLobby)
                        //{
                            foreach (Client _client in Server.clients)
                            {
                                if (_client != client)
                                {
                                    //if ((client.account.isInGame && client.account.isInGame) || (client.account.isInLobby && client.account.isInLobby))
                                    //{
                                        if(client.account.char_rank == "Admin" || client.account.char_rank == "Developper")
                                        {
                                            _client.Send(new BM_SC_CHAT_MESSAGE(msgSender.Replace("\0", string.Empty), (sbyte)Ava1.lib.core.net.enums.packet.ChatTypeEnums.ChatType.CHAT_GM, (short)msgLenght, msg));
                                        }
                                        _client.Send(new BM_SC_CHAT_MESSAGE(msgSender.Replace("\0", string.Empty), (sbyte)msgType, (short)msgLenght, msg));
                                    //}
                                }
                          //  }
                      //  }
                        if (client.account.isInLobbyRoom)
                        {
                            if (client.account.roomId != 0)
                            {
                                var thisRoom = RoomParty.FirstOrDefault(x => x.Id == client.account.roomId);
                                if (thisRoom != null)
                                {
                                    thisRoom.Send(new BM_SC_CHAT_MESSAGE(msgSender.Replace("\0", string.Empty), (sbyte)msgType, (short)msgLenght, msg));
                                }
                            }
                        }
                        client.Send(new BM_SC_CHAT_USER());
                        console.cw(console.type.Info, "{0}: '{1}'", client.account.char_name, msg);
                    }
                }
            }
        }

      

        [MMO((UInt16)MmoServerEnums.MM_SC_MSN)]
        public static void HandleMsnInitialization(Client client, Message message)
        {
            client.Send(new MM_SC_MSN());
            console.cw(console.type.Info, "Initialized MSN instance");
        }

        [MMO((UInt16)LobbyServerEnums.BM_SC_UNKNOW_INFO)]
        public static void HandleRoomUnknowInfo(Client client, Message message)
        {

        }

       
    }
}
