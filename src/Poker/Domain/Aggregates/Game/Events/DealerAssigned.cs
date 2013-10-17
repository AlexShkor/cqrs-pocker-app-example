using Poker.Domain.Aggregates.Game.Data;
using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Game.Events
{
    public class DealerAssigned : Event
    {
        public string GameId { get; set; }
        public PlayerInfo Dealer { get; set; }
        public PlayerInfo SmallBlind { get; set; }
        public PlayerInfo BigBlind { get; set; }
    }
}