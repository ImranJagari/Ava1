using System;
using System.Collections.Generic;

using Ava1.lib.core.io;

namespace Ava1.streetgears.network.mmo.engine.room
{
    public class RoomParty
    {
        public Int32 Id;
        public List<Client> Client;
        public Int32 charType;
        public Int32 isAdmin;
        public Int16 mapId;
        public Int32 currentPlayers;
        public Boolean hasStarted;
        public SByte license;

        public RoomParty(int _id, int _charType, int _isAdmin, short _mapId)
        {
            Id = _id;
            Client = new List<Client>();
            charType = _charType;
            isAdmin = _isAdmin;
            mapId = _mapId;
            currentPlayers = 0;
            hasStarted = false;
            license = 0;
        }

        public void AddToParty(Client _client)
        {
            Client.Add(_client);
        }

        public void RemoveFromParty(Client _client)
        {
            Client.Remove(_client);
        }

        public void Send(PacketBase packet)
        {
            Client.ForEach(client => client.Send(packet));
        }
    }
}
