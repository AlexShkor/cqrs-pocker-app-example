using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Events
{
    public class BidMade: Event
    {
        public string GameId { get; set; }

        public BidInfo Bid { get; set; }
    }
}