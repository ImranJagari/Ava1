namespace Ava1.lib.core.enums.network.global
{
    public class LobbyRoomEnums
    {
        public enum MapId
        {
            RANDOM = 0,
            GRIND_ROLLER = 57,
            CROSS_LINK = 59,
            GRIND_CROSS = 61,
            TRIESTE_EASY = 119,
            ROLLER_STADIUM = 67,
            FORBIDDEN_CITY = 107,
            STAR_TRACK = 63,
            MIRACLE_EASY = 120,
            PARAKA_EASY = 121,
            TRIANGLE_FARM = 122,
            LOST_ISLAND = 123,
        }

        public enum UserEntryInfoStatus
        {
             USER_LEADER = 5,
             USER_LEAVE = 4,
             USER_ENTER = 3,
             USER_READY = 2,
             USER_TEAM = 1,
        }

        public enum UserRoomStatus
        {
            USER_NORMAL = 0,
            USER_INVENTORY = 1,
        }
    }
}
