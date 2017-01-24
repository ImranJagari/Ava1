using System;

using Ava1.lib.core.enums.network.op;
using Ava1.lib.core.io;

namespace Ava1.streetgears.network.auth.message.packet
{
    public class TM_SC_SERVER_LIST : PacketBase
    {
        public override UInt16 packetOp { get; set; }
        public static Int16 unk0 = 1;
        public static Int16 serverId;
        public static Int16 msgServerId;
        public static Int16 lobbyServerId;
        public static Int16 mmoServerId;
        public static String msgIP = "";
        public static String lobbyIP = "";
        public static String mmoIP = "";
        public static Int32 msgPort;
        public static Int32 lobbyPort;
        public static Int32 mmoPort;
        public static Int16 currentClients;
        public static Int16 maxClients;

        public TM_SC_SERVER_LIST(short _serverId, short _msgServerId, short _lobbyServerId, short _mmoServerId, string _msgIP, string _lobbyIP, string _mmoIP, short _msgPort, short _lobbyPort, short _mmoPort, short _currentClients, short _maxClients)
        {
            serverId = _serverId;
            msgServerId = _msgServerId;
            lobbyServerId = _lobbyServerId;
            mmoServerId = _mmoServerId;
            msgIP = _msgIP;
            lobbyIP = _lobbyIP;
            mmoIP = _mmoIP;
            msgPort = _msgPort;
            lobbyPort = _lobbyPort;
            mmoPort = _mmoPort;
            currentClients = _currentClients;
            maxClients = _maxClients;
        }

        public override byte[] Pack()
        {
            var message = new Message((UInt16)AuthServerEnums.TM_SC_SERVER_LIST + 8);

            message.PutShort(unk0);
            message.PutShort(serverId);
            message.PutShort(msgServerId);
            message.PutShort(lobbyServerId);
            message.PutShort(mmoServerId);
            message.PutString(msgIP.PadRight(16, '\0'));
            message.PutString(lobbyIP.PadRight(16, '\0'));
            message.PutString(mmoIP.PadRight(16, '\0'));
            message.PutInt(msgPort);
            message.PutInt(lobbyPort);
            message.PutInt(mmoPort);
            message.PutShort(currentClients);
            message.PutShort(maxClients);

            return MessageManager.Build(message, true);
        }
    }
}
