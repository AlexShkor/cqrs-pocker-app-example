using Poker.Domain.Aggregates.Game.Data;
using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Game.Events
{
    public class NextPlayerTurned: Event
    {
        public string GameId { get; set; }
        public PlayerInfo Player { get; set; }
    }
}