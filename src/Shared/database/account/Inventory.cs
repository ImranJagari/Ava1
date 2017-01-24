using System;

namespace Ava1.shared.database.account
{
    public class Inventory
    {
        public Int32 numberId;
        public Int32 itemId;
        public Int32 tradeStatus;
        public Int32 duration;
        public Int32 equiped;

        public Inventory(int _numberId, int _itemId, int _tradeStatus, int _duration, int _equiped)
        {
            numberId = _numberId;
            itemId = _itemId;
            tradeStatus = _tradeStatus;
            duration = _duration;
            equiped = _equiped;
        }
    }
}
