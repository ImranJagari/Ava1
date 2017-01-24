using System;
using System.Linq;

using Ava1.streetgears.network.mmo.message.packet.mmo;
using Ava1.streetgears.network.mmo.message.packet.social;
using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.config;
using Ava1.lib.core.enums.network.global;
using Ava1.lib.core.utils;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.handlers
{
    public class HandleGlobalMmoInteractions
    {
        [MMO((UInt16)MmoServerEnums.BM_SC_SET_SESSION_MESSAGE)] 
        public static void HandleSessionMessage(Client client, Message message)
        {
            var server = shared.database.server.Server.Servers.FirstOrDefault(x => x.name == Config.elements["Server"]["Name"]);
            var motd = Config.elements["Game"]["Motd"].ToString();

            if (!client.account.isMotdSent)
            {
                client.Send(new BM_SC_CHAT_MESSAGE(string.Empty, (sbyte)ChatTypeEnums.ChatType.CHAT_SYSTEM_INFO, (short)motd.Length, motd));
                Log.WriteLog(Log.Type.Info, "Server said '{0}' to '{1}'", motd, client.account.char_name);
                client.account.isMotdSent = true;
            }
            server.channel_currentnum++;
            server?.UpdateServer();
        }

        [MMO((UInt16)MmoServerEnums.BM_SC_QUEST_DAY_COIN)]
        public static void HandleDailyQuestCoin(Client client, Message message)
        {
            if (client.account.char_coin <= 949)
            {
                client.account.char_coin += 50;
            }
            else
            {
                client.account.char_coin = 999;
            }
            client.Send(new BM_SC_REWARD_QUEST(0, 0, 0, 50));
            client.Send(new BM_SC_QUEST_DAY_COIN(50));
            client?.account.UpdateAccount();
        }

        [MMO((UInt16)MmoServerEnums.BM_SC_EXCHANGE_MONEY)]
        public static void HandleExchangeMoney(Client client, Message message)
        {
            if (message.BytesAvaible >= 2)
            {
                var coin1 = message.GetByte();
                var coin2 = message.GetByte();

                client.account.char_gamecash += (coin2 | coin1 << 8 / 2) / 2;
                client.account.char_coin -= (coin2 | coin1 << 8);

                client.Send(new BM_SC_EXCHANGE_MONEY());
                client.Send(new BM_SC_BALANCE_INFO(client.account.char_gamecash, client.account.char_coin, client.account.char_cash, client.account.char_questpoint));
                client.Send(new BM_SC_CASH_BALANCE_INFO(client.account.char_cash));

                Log.WriteLog(Log.Type.Info, "'{0}' exchanged money, lost '{1}' coins, gain '{2}' game cash", client.account.char_name, (coin2 | coin1 << 8), (coin2 | coin1 << 8 / 2) / 2);
            }
        }

        [MMO((UInt16)MmoServerEnums.BM_CS_UPDATE_QP)]
        public static void HandleQpUpdate(Client client, Message message)
        {
            client.Send(new BM_CS_UPDATE_QP(client.account.char_questpoint));
            Log.WriteLog(Log.Type.Info, "Updated quest points ({0}) for '{1}'", client.account.char_questpoint, client.account.char_name);
        }

        [MMO((UInt16)MmoServerEnums.BM_SC_MMO_EVENT_MESSAGE)]
        public static void HandleEventMessage(Client client, Message message)
        {
            if (message.BytesAvaible >= 4)
            {
                var unk1 = message.GetInt();
                client.Send(new BM_SC_MMO_EVENT_MESSAGE(unk1));
            }
        }

        [MMO((UInt16)MmoServerEnums.ID_BZ_SC_ENTER_LOBBY)]
        public static void HandleLobbyEntry(Client client, Message message)
        {
            client.account.isInLobby = true;
            client.account.isInGame = false;
            client.account.isInLobbyRoom = false;

            client.Send(new ID_BZ_SC_ENTER_LOBBY());

            Log.WriteLog(Log.Type.Info, "{0} joined lobby waiting room", client.account.char_name);
        }
    }
}
