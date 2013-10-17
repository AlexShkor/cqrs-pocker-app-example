using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Game.Events
{
    public class PlayerJoined: Event
    {
        public int Position { get; set; }

        public string UserId { get; set; }

        public long Cash { get; set; }
    }
}