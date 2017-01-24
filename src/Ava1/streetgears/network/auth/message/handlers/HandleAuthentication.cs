using System;
using System.Linq;

using Ava1.lib.core.enums.network.op;
using Ava1.shared.database.account;
using Ava1.lib.core.utils;
using Ava1.lib.core.config;
using Ava1.lib.core.enums.network.global;
using Ava1.streetgears.network.auth.message.packet;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.auth.message.handlers
{
    public class HandleAuthentication
    {
        [Auth((UInt16)AuthServerEnums.TS_XX_KEEP_ALIVE)]
        public static void HandleKeepAlive(Client client, Message message) { }

        [Auth((UInt16)AuthServerEnums.TS_SC_VERSION)]
        public static void HandleVersion(Client client, Message message) { }

        [Auth((UInt16)AuthServerEnums.TS_SC_ACCOUNT)]
        public static void HandleLogin(Client client, Message message)
        {
            if (message.BytesAvaible >= 38)
            {
                var username = message.GetString(17);
                var unk = message.GetShort();
                var password = message.GetString(17);

                var account = Account.getAccount(username, password);
                if (account != null)
                {
                    if (!account.isInGame && !account.isInLobby && !account.isInLobbyRoom)
                    {
                        var sessionKey = Functions.Random(17, false);
                        AccountTransition.addAccount(account, sessionKey);

                        client.account = account;

                        if (client.account.first_login == 1)
                        {
                            Log.WriteLog(Log.Type.Info, "{0} logged in with session key '{1}'", client.account.char_name, sessionKey);
                        }

                        if (client.account.char_rank == "Banned")
                        {
                            client.Send(new TM_SC_RESULT((byte)AuthenticationEnums.ResultId.msg_server_denied));
                        }
                        else
                        {
                            client.Send(new TS_SC_WE_LOGIN(sessionKey));

                            HandleServerList(client, message);
                            HandleSelectServer(client, message);
                        }
                    }
                    else
                    {
                        var thisMmoClient = mmo.Server.clients.FirstOrDefault(x => x.account.char_name == client.account.char_name);
                        var thisLobbyClient = lobby.Server.clients.FirstOrDefault(x => x.account.char_name == client.account.char_name);

                        client.Disconnect();
                        thisMmoClient.Disconnect();
                        thisLobbyClient.Disconnect();

                        client.Send(new TM_SC_RESULT((byte)AuthenticationEnums.ResultId.msg_server_already_exist));
                        Log.WriteLog(Log.Type.Info, "'{0}:{1}' tried to connect on connected account '{2}:{3}'", client.Ip, client.Port, username.Replace("\0", string.Empty), password.Replace("\0", string.Empty));
                    }
                }
                else
                {
                    client.Send(new TM_SC_RESULT((byte)AuthenticationEnums.ResultId.msg_server_not_exist));
                    Log.WriteLog(Log.Type.Info, "'{0}:{1}' failed to connect with creds '{2}:{3}'", client.Ip, client.Port, username.Replace("\0", string.Empty), password.Replace("\0", string.Empty));
                }
            }
        }

        public static void HandleServerList(Client client, Message message)
        {
            client.Send(new TM_SC_SERVER_LIST(1, 2, 3, 4, Config.elements["Msg"]["Ip"], Config.elements["Lobby"]["Ip"], Config.elements["MMO"]["Ip"], Convert.ToInt16(Config.elements["Msg"]["Port"]), Convert.ToInt16(Config.elements["Lobby"]["Port"]), Convert.ToInt16(Config.elements["MMO"]["Port"]), 1, Convert.ToInt16(Config.elements["Game"]["MaxClient"])));
        }

        public static void HandleSelectServer(Client client, Message message)
        {
            var server = shared.database.server.Server.Servers.FirstOrDefault(x => x.name == Config.elements["Server"]["Name"]);
            client.Send(new TM_SC_SELECT_SERVER((sbyte)server.channel_status));
        }
    }
}
