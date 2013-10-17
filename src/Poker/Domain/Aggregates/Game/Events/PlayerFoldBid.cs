using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Game.Events
{
    public class PlayerFoldBid : Event
    {
        public string GameId { get; set; }
        public string UserId { get; set; }
        public int Position { get; set; }
    }

    public class PlayerCheckedBid : Event
    {
        public string GameId { get; set; }
        public string UserId { get; set; }
        public int Position { get; set; }
    }
}