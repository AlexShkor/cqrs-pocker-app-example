using PAQK.Domain.Aggregates.Game.Data;
using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Events
{
    public class NextPlayerTurned: Event
    {
        public string GameId { get; set; }
        public PlayerInfo Player { get; set; }
    }
}