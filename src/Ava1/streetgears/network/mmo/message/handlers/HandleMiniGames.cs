using System;

using Ava1.streetgears.network.mmo.message.packet.mmo;
using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.utils;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.message.handlers
{
    public class HandleMiniGames
    {
        [MMO((UInt16)MmoServerEnums.BM_SC_MINIGAME_START)]
        public static void HandleMiniGameStart(Client client, Message message)
        {
            if (message.BytesAvaible >= 5)
            {
                var slalomId = message.GetShort();
                var unk1 = message.GetBool();
                var unk2 = message.GetShort();

                client.Send(new BM_SC_MINIGAME_START(client.account.char_name));
                Log.WriteLog(Log.Type.Info, "{0} started minigame '{1}'", client.account.char_name, slalomId);
            }
        }

        [MMO((UInt16)MmoServerEnums.BM_SC_MINIGAME_FINISH)]
        public static void HandleMiniGameEnd(Client client, Message message)
        {
            if (message.BytesAvaible >= 7)
            {
                var slalomId = message.GetShort();
                var slalomReversed = message.GetBool(); //prob false
                var unk2 = message.GetShort();
                var slalomDuration = message.GetShort();

                client.Send(new BM_SC_MINIGAME_FINISH());
                Log.WriteLog(Log.Type.Info, "{0} fisnihed minigame '{1}' in '{2}ms'", client.account.char_name, slalomId, slalomDuration);
            }
        }
    }
}
