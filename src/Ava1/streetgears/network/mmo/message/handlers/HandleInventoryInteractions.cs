using System;
using System.Linq;

using Ava1.streetgears.network.mmo.message.packet.mmo;
using Ava1.streetgears.network.mmo.message.packet.room;
using Ava1.lib.core.io;
using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.enums.network.global;
using Ava1.lib.core.utils;

namespace Ava1.streetgears.network.mmo.message.handlers
{
    public class HandleInventoryInteractions
    {
        [MMO((UInt16)MmoServerEnums.BM_SC_ENTER_INVENTORY)]
        public static void HandleInventoryEntry(Client client, Message message)
        {
            if (client.account.isInLobbyRoom)
            {
                var thisRoom = HandleRoomInteractions.RoomParty.FirstOrDefault(x => x.Client.Contains(client));
                if (thisRoom != null)
                {
                    if (client.account.isReady)
                        client.account.isReady = false;

                    if (client.account.roomStatus == (int)LobbyRoomEnums.UserRoomStatus.USER_NORMAL)
                        client.account.roomStatus = (int)LobbyRoomEnums.UserRoomStatus.USER_INVENTORY;

                    thisRoom.Send(new BM_SC_USER_INFO(client.Ip, client.account.char_name, (sbyte)client.account.roomSlot, (sbyte)client.account.char_type, (int)LobbyRoomEnums.UserEntryInfoStatus.USER_ENTER, 0, (sbyte)client.account.roomSlot, Convert.ToSByte(client.account.isReady), (sbyte)client.account.roomStatus));
                }
            }
            client.Send(new BM_SC_ENTER_INVENTORY());
        }

        [MMO((UInt16)MmoServerEnums.BM_SC_INVENTORY)]
        public static void HandleInventoryItems(Client client, Message message)
        {
            client.Send(new BM_SC_INVENTORY(client));
        }

        [MMO((UInt16)MmoServerEnums.BM_SC_SELECT_ITEM)]
        public static void HandleItemSelection(Client client, Message message)
        {
            if (message.BytesAvaible >= 16)
            {
                var itemId = message.GetInt();
                var itemWearInfo = message.GetInt();
                var itemSlotNumber = message.GetInt();
                var itemEquip = message.GetInt();
                var charInventory = string.Empty;

                var thisItem = client.account.Item.FirstOrDefault(x => x.itemId == itemId);
                if (thisItem != null)
                {
                    var comboId = $"{thisItem.itemId},{thisItem.tradeStatus},{thisItem.duration},{thisItem.equiped}";
                    var comboId2 = $"{thisItem.itemId},{thisItem.tradeStatus},{thisItem.duration},{itemEquip}";
                    if (client.account.char_itemlist.Contains($"{comboId}"))
                    {
                        charInventory = client.account.char_itemlist.Replace(comboId, comboId2);
                    }
                    thisItem.equiped = itemEquip;
                    client.account.char_itemlist = charInventory;
                    client.Send(new BM_SC_SELECT_ITEM(itemId, itemWearInfo));
                    client.Send(new BM_SC_INVENTORY(client));
                    client.account?.UpdateAccount();
                }
            }
        }

        [MMO((UInt16)MmoServerEnums.BM_SC_DELETE_ITEM)]
        public static void HandleItemDelete(Client client, Message message)
        {
            if (message.BytesAvaible >= 8)
            {
                var itemId = message.GetInt();
                var itemSlot = message.GetInt();
                var charInventory = string.Empty;

                var selectedItem = client.account.Item.FirstOrDefault(x => x.itemId == itemId);
                if (selectedItem != null)
                {
                    var comboId = $"{selectedItem.itemId},{selectedItem.tradeStatus},{selectedItem.duration},{selectedItem.equiped}";
                    if (client.account.char_itemlist.Contains($"{comboId}|"))
                    {
                        charInventory = client.account.char_itemlist.Replace($"{comboId}|", string.Empty);
                    }
                    else if (client.account.char_itemlist.Contains($"|{comboId}"))
                    {
                        charInventory = client.account.char_itemlist.Replace($"|{comboId}", string.Empty);
                    }
                    if (selectedItem.equiped == 1)
                    {
                        client.Send(new BM_SC_DELETE_ITEM_RESULT("EquipItem_Delete_Err"));
                    }
                    else
                    {
                        client.account.Item.Remove(selectedItem);
                        client.account.char_itemlist = charInventory;
                        client.account?.UpdateAccount();
                        client.Send(new BM_SC_DELETE_ITEM(itemSlot));
                    }
                    Log.WriteLog(Log.Type.Info, "{0} deleted item '{1}' from database", client.account.char_name, itemId);
                }
            }
        }

        [MMO((UInt16)MmoServerEnums.BM_SC_ITEM_USE)]
        public static void HandleItemUse(Client client, Message message)
        {
            if (message.BytesAvaible >= 6)
            {
                var itemSlot = message.GetInt();
                var itemCount = message.GetShort();

                client.Send(new BM_SC_ITEM_USE(itemSlot, itemCount));
            }
        }

        [MMO((UInt16)MmoServerEnums.BM_SC_LEAVE_INVENTORY)]
        public static void HandleInventoryExit(Client client, Message message)
        {
            if (client.account.isInLobbyRoom)
            {
                var thisRoom = HandleRoomInteractions.RoomParty.FirstOrDefault(x => x.Client.Contains(client));
                if (thisRoom != null)
                {
                    if (client.account.isReady)
                        client.account.isReady = false;

                    if (client.account.roomStatus == (int)LobbyRoomEnums.UserRoomStatus.USER_INVENTORY)
                        client.account.roomStatus = (int)LobbyRoomEnums.UserRoomStatus.USER_NORMAL;

                    thisRoom.Send(new BM_SC_USER_INFO(client.Ip, client.account.char_name, (sbyte)client.account.roomSlot, (sbyte)client.account.char_type, (int)LobbyRoomEnums.UserEntryInfoStatus.USER_ENTER, 0, (sbyte)client.account.roomSlot, Convert.ToSByte(client.account.isReady), (sbyte)client.account.roomStatus));
                }
            }
            if (client.account.isInLobby)
            {
                HandleGlobalMmoInteractions.HandleLobbyEntry(client, message);
            }
            client.Send(new BM_SC_LEAVE_INVENTORY());
        }

        [MMO((UInt16)MmoServerEnums.BM_SC_SELECT_TRICK)]
        public static void HandleTrickSelection(Client client, Message message)
        {
            if (message.BytesAvaible >= 2)
            {
                var trickId = message.GetShort();
                var charTricks = string.Empty;

                if (client.account.Trick != null)
                {
                    var selectedTrick = client.account.Trick.FirstOrDefault(x => x.Id == trickId);
                    if (selectedTrick != null)
                    {
                        var comboId = $"{selectedTrick.Id},{selectedTrick.Level},{selectedTrick.Apply}";
                        if (client.account.char_tricklist.Contains(comboId))
                        {
                            if (selectedTrick.Apply == 1)
                            {
                                charTricks = client.account.char_tricklist.Replace(comboId, $"{selectedTrick.Id},{selectedTrick.Level},0");
                                client.Send(new BM_SC_SELECT_TRICK(trickId)); selectedTrick.Apply = 0;
                            }
                            else if (selectedTrick.Apply == 0)
                            {
                                charTricks = client.account.char_tricklist.Replace(comboId, $"{selectedTrick.Id},{selectedTrick.Level},1");
                                selectedTrick.Apply = 1;
                            }
                            client.account.char_tricklist = charTricks;
                        }
                    }
                    client.account?.UpdateAccount();
                }
            }
        }

        [MMO((UInt16)MmoServerEnums.BS_SC_EXPAND_SLOT)]
        public static void HandleSlotExpandation(Client client, Message message)
        {
            if (client.account.char_gamecash >= 500)
            {
                client.account.char_gamecash -= 500;
                client.Send(new BM_SC_BALANCE_INFO(client.account.char_gamecash, client.account.char_coin, client.account.char_cash, client.account.char_questpoint));
                client.Send(new BS_SC_EXPAND_SLOT());
                client.account?.UpdateAccount();
            }
            else
            {
                client.Send(new BS_SC_EXPAND_SLOT_RESULT("NOT_ENOUGH_GAME_MONEY"));
            }
        }
    }
}
