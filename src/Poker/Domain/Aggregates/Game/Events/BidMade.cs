using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Game.Events
{
    public class BidMade: Event
    {
        public string GameId { get; set; }

        public BidInfo Bid { get; set; }

        public BidTypeEnum BidType { get; set; }
    }

    public enum BidTypeEnum
    {
        Call,
        Raise
    }
}