using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Game.Events
{
    public class BiddingFinished: Event
    {
        public string GameId { get; set; }
        public long Bank { get; set; }
        public long MinRaise { get; set; }
    }
}