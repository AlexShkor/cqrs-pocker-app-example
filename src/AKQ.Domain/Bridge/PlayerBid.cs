using System;

namespace AKQ.Domain
{
    [Serializable]
    public class PlayerBid
    {
        private readonly Bid _bid;
        private readonly PlayerPosition _position;

        public PlayerBid(PlayerPosition position, Bid bid)
        {
            _bid = bid;
            _position = position;
        }

        public Bid Bid
        {
            get { return _bid; }
        }

        public PlayerPosition Position
        {
            get { return _position; }
        }
    }
}