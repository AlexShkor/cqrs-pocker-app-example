using PAQK.Domain.Aggregates.Game.Data;
using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Events
{
    public class DealerAssigned : Event
    {
        public string GameId { get; set; }
        public PlayerInfo Dealer { get; set; }
        public PlayerInfo SmallBlind { get; set; }
        public PlayerInfo BigBlind { get; set; }
    }
}