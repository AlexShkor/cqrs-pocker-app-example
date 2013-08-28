using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Events
{
    public class BlindBidsMade : Event
    {
        public string GameId { get; set; }
        public BidInfo SmallBlind { get; set; }
        public BidInfo BigBlind { get; set; }
    }
}