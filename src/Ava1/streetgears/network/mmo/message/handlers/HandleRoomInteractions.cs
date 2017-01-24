using System;
using System.Linq;
using System.Collections.Generic;

using Ava1.lib.core.io;
using Ava1.lib.core.utils;
using Ava1.lib.core.config;
using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.enums.network.global;
using Ava1.streetgears.network.mmo.message.packet.room;

namespace Ava1.streetgears.network.mmo.message.handlers
{
    public class HandleRoomInteractions
    {
        public static List<engine.room.Room> RoomList = new List<engine.room.Room>();
        public static List<engine.room.RoomPlayers> RoomPlayers = new List<engine.room.RoomPlayers>();
        public static List<engine.room.RoomParty> RoomParty = new List<engine.room.RoomParty>();

        public static Int32 roomId = 1;

        [MMO((UInt16)LobbyServerEnums.BM_SC_GET_ROOMLIST)]
        public static void HandleRoomList(Client client, Message message)
        {
            client.Send(new BM_SC_GET_ROOM_LIST((short)RoomList.Count));
        }

        [MMO((UInt16)LobbyServerEnums.BM_SC_CREATE_ROOM)]
        public static void HandleRoomCreation(Client client, Message message)
        {
            if (message.BytesAvaible >= 80)
            {
                var roomName = message.GetString(15);
                var unk2 = message.GetShort();
                var unk3 = message.GetShort();
                var unk4 = message.GetShort();
                var unk5 = message.GetShort();
                var unk7 = message.GetByte();
                var roomPassword = message.GetString(4);
                var unk8 = message.GetString(48);
                var roomMaximumPlayers = message.GetByte();
                var roomMode = message.GetInt(); 
                var roomLicense = message.GetByte();

                if (!RoomList.Contains(RoomList.FirstOrDefault(x => x.Name == roomName)))
                {
                    if (roomId < 50)
                    {
                        client.account.roomSlot = 0;
                        client.account.isInLobbyRoom = true;
                        client.account.isInLobby = false;
                        client.account.isLeader = true;
                        client.account.isReady = true;

                        client.account.roomEntry = (int)LobbyRoomEnums.UserEntryInfoStatus.USER_LEADER;
                        client.account.roomSlot = 1;

                        RoomList.Add(new engine.room.Room(roomId, roomName.Replace("\0", string.Empty), roomPassword, roomMode, 0, (sbyte)roomMaximumPlayers, (sbyte)roomLicense, (sbyte)roomLicense));
                        RoomParty.Add(new engine.room.RoomParty(roomId, client.account.char_type, 0, 0));

                        var thisRoom = RoomParty.FirstOrDefault(x => x.Id == roomId);
                        var thisRoom2 = RoomList.FirstOrDefault(x => x.Id == roomId);

                        thisRoom.AddToParty(client);
                        thisRoom.currentPlayers += 1;
                        thisRoom.license = (sbyte)roomLicense;

                        thisRoom2.currentPlayers += 1;

                        Log.WriteLog(Log.Type.Info, "{0} created room 'Name={1}, Password={2}, Maximum Players={3}, Mode={4}, License={5}'", client.account.char_name, roomName.Replace("\0", string.Empty), roomPassword.Replace("\0", string.Empty), roomMaximumPlayers, roomMode, roomLicense);

                        client.Send(new BM_SC_CREATE_ROOM(roomId, Config.elements["Relay"]["Ip"], Int16.Parse(Config.elements["Relay"]["Port"]), (sbyte)new Random().Next(1, 2), 5000));

                        foreach(Client thisClient in Server.clients)
                        {
                            if(thisClient.account.isInLobby)
                            {
                                thisClient.Send(new BM_SC_GET_ROOM_LIST((short)RoomList.Count));
                            }
                        }
                    }
                    else
                    {
                        Log.WriteLog(Log.Type.Info, "Can't create more room");
                    }
                }
                else
                {
                    Log.WriteLog(Log.Type.Info, "{0} tried to create already existing room '{1}'", roomName);
                }
                roomId++;
            }
        }

        [MMO((UInt16)LobbyServerEnums.BM_SC_ENTER_ROOM)]
        public static void HandleRoomEntry(Client client, Message message)
        {
            if (message.BytesAvaible >= 4)
            {
                var roomId = message.GetInt();

                var room = RoomList.FirstOrDefault(x => x.Id == roomId);
                if (room != null)
                {
                    if (room.currentPlayers < 8)
                    {
                        var thisRoom = RoomParty.FirstOrDefault(x => x.Id == roomId);
                        if (thisRoom != null)
                        {
                            if (!thisRoom.hasStarted)
                            {
                                if (thisRoom.license == client.account.char_licence)
                                {
                                    client.account.isReady = false;
                                    client.account.isInLobby = false;
                                    client.account.isInLobbyRoom = true;
                                    client.account.roomId = roomId;

                                    client.account.roomEntry = (int)LobbyRoomEnums.UserEntryInfoStatus.USER_ENTER;
                                    client.account.roomStatus = (int)LobbyRoomEnums.UserRoomStatus.USER_NORMAL;

                                    thisRoom.AddToParty(client);

                                    thisRoom.currentPlayers++;
                                    thisRoom.mapId = 0;

                                    room.currentPlayers++;

                                    client.account.roomSlot = thisRoom.currentPlayers;

                                    client.Send(new BM_SC_ENTER_ROOM(Config.elements["Relay"]["Ip"], Int16.Parse(Config.elements["Relay"]["Port"]), (sbyte)new Random().Next(1, 2), 5000));

                                    foreach (Client _client in thisRoom.Client)
                                    {
                                        _client.Send(new BM_SC_USER_INFO(client.Ip, client.account.char_name, (sbyte)client.account.roomSlot, (sbyte)client.account.char_type, (sbyte)client.account.roomEntry, Convert.ToSByte(client.account.isAdmin ? 0 : 1), (sbyte)client.account.roomSlot, Convert.ToSByte(client.account.isReady), (sbyte)client.account.roomStatus));
                                        client.Send(new BM_SC_USER_INFO(_client.Ip, _client.account.char_name, (sbyte)_client.account.roomSlot, (sbyte)_client.account.char_type, (sbyte)_client.account.roomEntry, Convert.ToSByte(_client.account.isAdmin ? 0 : 1), (sbyte)_client.account.roomSlot, Convert.ToSByte(_client.account.isReady), (sbyte)_client.account.roomStatus));
                                    }
                                    HandleLobbyInteractions.HandleCharacterInfo(client, message);

                                    Log.WriteLog(Log.Type.Info, "{0} joined room 'Id={1}, Current Players={2}'", client.account.char_name, thisRoom.Id, thisRoom.currentPlayers);
                                }
                                else
                                {
                                    client.Send(new BM_SC_ENTER_ROOM_RESULT("B_NOT_ENOUGH_LICENSE"));
                                }
                            }
                            else
                            {
                                client.Send(new BM_SC_ENTER_ROOM_RESULT("ALREADY_STARTED"));
                            }
                        }
                    }
                    else
                    {
                        client.Send(new BM_SC_ENTER_ROOM_RESULT("USER_EXCESSED"));
                    }
                }
            }
        }

        [MMO((UInt16)LobbyServerEnums.BM_SC_EDIT_ROOM)]
        public static void HandleRoomEdit(Client client, Message message)
        {
            if (client.account != null)
            {
                if (client.account.isLeader)
                {
                    var unk0 = message.GetByte(); 
                    var roomName = message.GetString(24).Replace("\0", string.Empty);
                    var roomPassword = message.GetString(4).Replace("\0", string.Empty);

                    var thisRoom = RoomParty.FirstOrDefault(x => x.Client.Contains(client));
                    var thisRoom2 = RoomList.FirstOrDefault(x => x.Id == thisRoom.Id);

                    if (roomName != string.Empty)
                    {
                        thisRoom2.Name = roomName;
                        thisRoom2.Password = roomPassword;
                    }
                    thisRoom.Send(new BM_SC_EDIT_ROOM());
                }
            }
        }

        [MMO((UInt16)LobbyServerEnums.BM_SC_SELECT_MAP)]
        public static void HandleMapSelection(Client client, Message message)
        {
            if (client.account != null)
            {
                if (client.account.isLeader)
                {
                    if (message.BytesAvaible >= 2)
                    {
                        var mapId = message.GetShort();

                        var thisRoom = RoomParty.FirstOrDefault(x => x.Client.Contains(client));
                        if (thisRoom != null)
                        {
                            if (mapId == 0)
                            {
                                //var values = Enum.GetValues(typeof(LobbyRoomEnums.MapId));
                                //mapId = (short)values.GetValue(new Random().Next(values.Length));
                                mapId = (short)LobbyRoomEnums.MapId.FORBIDDEN_CITY;
                            }
                            thisRoom.mapId = mapId;
                        }
                        thisRoom.Send(new BM_SC_SELECT_MAP(thisRoom.mapId));
                        thisRoom.Send(new BM_SC_MAP_INFO(thisRoom.mapId));
                        Log.WriteLog(Log.Type.Info, "{0} selected map id '{1}'", client.account.char_name, mapId);
                    }
                }
            }
        }

        [MMO((UInt16)LobbyServerEnums.BM_SC_LEAVE_ROOM)]
        public static void HandleRoomExit(Client client, Message message)
        {
            if (client.account != null)
            {
                client.Send(new BM_SC_LEAVE_ROOM());

                client.account.roomId = -1;
                client.account.isReady = false;
                client.account.isInLobbyRoom = false;
                client.account.isInLobby = true;

                var thisRoom = RoomParty.FirstOrDefault(x => x.Client.Contains(client));
                if (thisRoom != null)
                {
                    var thisRoom2 = RoomList.FirstOrDefault(x => x.Id == thisRoom.Id);

                    thisRoom2.currentPlayers -= 1;
                    thisRoom.currentPlayers -= 1;

                    if (thisRoom2.currentPlayers == 0)
                    {
                        RoomList.Remove(thisRoom2);
                    }
                    if (thisRoom.Client.Count == 0)
                    {
                        RoomParty.Remove(thisRoom);
                    }

                    if (client.account.isLeader)
                    {
                        if (thisRoom != null)
                        {
                            thisRoom.RemoveFromParty(client);
                            thisRoom.Client[thisRoom.Client.Count].account.isLeader = true;
                            thisRoom.Client[thisRoom.Client.Count].account.roomStatus = (sbyte)LobbyRoomEnums.UserRoomStatus.USER_NORMAL;
                            thisRoom.Client[thisRoom.Client.Count].account.roomEntry = (sbyte)LobbyRoomEnums.UserEntryInfoStatus.USER_LEADER;
                            thisRoom.Send(new BM_SC_USER_INFO(thisRoom.Client[thisRoom.Client.Count].Ip, thisRoom.Client[thisRoom.Client.Count].account.char_name, (sbyte)thisRoom.Client[thisRoom.Client.Count].account.roomSlot, (sbyte)thisRoom.Client[thisRoom.Client.Count].account.char_type, (sbyte)thisRoom.Client[thisRoom.Client.Count].account.roomEntry, Convert.ToSByte(thisRoom.Client[thisRoom.Client.Count].account.isAdmin ? 0 : 1), (sbyte)thisRoom.Client[thisRoom.Client.Count].account.roomSlot, Convert.ToSByte(thisRoom.Client[thisRoom.Client.Count].account.isReady), (sbyte)thisRoom.Client[thisRoom.Client.Count].account.roomStatus));
                        }
                    }
                    else
                    {
                        if (thisRoom != null)
                        {
                            var thisClient = thisRoom.Client.FirstOrDefault(x => x.account.char_id == client.account.char_id);
                            if (thisClient != null)
                            {
                                thisClient.account.roomEntry = (sbyte)LobbyRoomEnums.UserEntryInfoStatus.USER_LEAVE;
                                thisRoom.Send(new BM_SC_USER_INFO(thisClient.Ip, thisClient.account.char_name, (sbyte)thisClient.account.roomSlot, (sbyte)thisClient.account.char_type, (sbyte)thisClient.account.roomEntry, Convert.ToSByte(thisClient.account.isAdmin ? 0 : 1), (sbyte)thisClient.account.roomSlot, Convert.ToSByte(thisClient.account.isReady), (sbyte)thisClient.account.roomStatus));
                            }
                            thisRoom.RemoveFromParty(client);
                        }
                    }
                }
            }
        }

        [MMO((UInt16)LobbyServerEnums.BM_SC_GAME_READY)]
        public static void HandleReadyGame(Client client, Message message)
        {
            if (client.account != null)
            {
                var thisRoom = RoomParty.FirstOrDefault(x => x.Client.Contains(client));
                if (thisRoom != null)
                {
                    if (!client.account.isReady)
                    {
                        client.account.isReady = true;
                    }
                    else
                    {
                        client.account.isReady = false;
                        client.Send(new BM_SC_GAME_READY());
                    }
                }
                thisRoom.Send(new BM_SC_USER_INFO(client.Ip, client.account.char_name, (sbyte)client.account.roomSlot, (sbyte)client.account.char_type, (sbyte)client.account.roomEntry, Convert.ToSByte(client.account.isAdmin ? 0 : 1), (sbyte)client.account.roomSlot, Convert.ToSByte(client.account.isReady), (sbyte)client.account.roomStatus));
            }
        }

        [MMO((UInt16)LobbyServerEnums.BM_SC_FINISH_RACE)]
        public static void HandleEndGame(Client client, Message message)
        {
            var thisRoom = RoomParty.FirstOrDefault(x => x.Client.Contains(client));

            client.Send(new BM_SC_END_GAME());

            thisRoom.hasStarted = false;

            Log.WriteLog(Log.Type.Info, "Game ended for room id '{0}'", client.account.char_name, thisRoom.Id);
        }

        [MMO((UInt16)LobbyServerEnums.BM_SC_START_GAME)]
        public static void HandleStartGame(Client client, Message message)
        {
            var encryptionKey = Functions.Random(16, false);
            var thisRoom = RoomParty.FirstOrDefault(x => x.Client.Contains(client));

            if (thisRoom != null)
            {
                thisRoom.hasStarted = true;

                if (thisRoom.mapId == 0)
                {
                    //var values = Enum.GetValues(typeof(LobbyRoomEnums.MapId));
                    //(short)values.GetValue(new Random().Next(values.Length));

                    thisRoom.mapId = (short)LobbyRoomEnums.MapId.FORBIDDEN_CITY;
                    thisRoom.Send(new BM_SC_SELECT_MAP(thisRoom.mapId));
                    thisRoom.Send(new BM_SC_MAP_INFO(thisRoom.mapId));
                }

                thisRoom.Send(new BM_SC_START_GAME(thisRoom, encryptionKey, (short)thisRoom.Client.Count));
                Log.WriteLog(Log.Type.Info, "Game started for room id '{0}'", thisRoom.Id);
            }
        }

        [MMO((UInt16)LobbyServerEnums.BM_SC_MAP_INFO)]
        public static void HandleMapInfo(Client client, Message message)
        {
            var thisRoom = RoomParty.FirstOrDefault(x => x.Client.Contains(client));
            client.Send(new BM_SC_MAP_INFO(thisRoom.mapId));
        }

        [MMO((UInt16)LobbyServerEnums.BM_SC_UNKNOW_INFO)]
        public static void HandleRoomUnknowInfo(Client client, Message message)
        {
            client.Send(new BM_SC_UNKNOW_INFO(client.account.char_name));
        }
    }
}
