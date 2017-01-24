using System;

using Ava1.streetgears.network.mmo.message.packet.mmo;
using Ava1.lib.core.io;
using Ava1.lib.core.utils;
using Ava1.lib.core.enums.network.op;
using Ava1.shared.database.account;

namespace Ava1.streetgears.network.mmo.message.handlers
{
    public class HandleGameLogin
    {
        [MMO((UInt16)MmoServerEnums.BM_SC_LOGIN)]
        public static void HandleLogin(Client client, Message message)
        {
            if (message.BytesAvaible >= 17)
            {
                var sessionKey = message.GetString(17);
                var account = AccountTransition.getAccount(sessionKey);
                if (account != null)
                {
                    client.account = account;
                    client.sessionKey = sessionKey;
                    client.Send(new BM_SC_LOGIN());
                    client.Send(new BM_SC_PLAYER_CHARACTER_LIST(client.account.char_name, (short)client.account.first_login, client.account.char_type));
                    Log.WriteLog(Log.Type.Info, "Loaded account Id:'{0}', Name:'{1}', SessionKey:'{2}'", client.account.char_id,  client.account.char_name, client.sessionKey);
                }
                else
                {
                    Log.WriteLog(Log.Type.Info, "Account for session key '{0}' cannot be found", sessionKey);
                }
            }
        }

        [MMO((UInt16)MmoServerEnums.BM_SC_SELECT_CHARACTER)]
        public static void HandleCharacterSelection(Client client, Message message)
        {
            client.Send(new BM_SC_SELECT_CHARACTER());
        }

        [MMO((UInt16)MmoServerEnums.BM_SC_PLAYER_INFO)]
        public static void HandlePlayerInfo(Client client, Message message)
        {
            if (!client.account.isInGame)
            {
                client.Send(new BM_SC_PLAYER_INFO(client.account.char_level));
                HandleTrickList(client, message);
                HandleChannelInteractions.HandleChannelList(client, message);
            }
        }

        public static void HandlePlayerLevelInfo(Client client, Message message)
        {
            if (!client.account.isInGame)
            {
                client.Send(new BM_SC_LEVEL_INFO(client.account.char_exp, client.account.char_level, client.account.char_licence));
            }
        }

        [MMO((UInt16)MmoServerEnums.BM_SC_TRICK_LIST)]
        public static void HandleTrickList(Client client, Message message)
        {
            if (!client.account.isInGame)
            {
                client.Send(new BM_SC_TRICK_LIST(client));
                Log.WriteLog(Log.Type.Info, "Loaded '{0}' tricks for '{1}'", client.account.Trick.Count, client.account.char_name);
            }
        }

        public static void HandleBalanceInfo(Client client, Message message)
        {
            if (!client.account.isInGame)
            {
                client.Send(new BM_SC_BALANCE_INFO(client.account.char_gamecash, client.account.char_coin, client.account.char_cash, client.account.char_questpoint));
                client.Send(new BM_SC_CASH_BALANCE_INFO(client.account.char_cash));
            }
        }
    }
}
