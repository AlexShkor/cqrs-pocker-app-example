using System.Collections.Generic;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Game.Events
{
    public class GameFinished : Event
    {
        public string GameId { get; set; }
        public List<WinnerInfo> Winners { get; set; }

        public PlayerInfo Winner { get; set; }

        public long Bank { get; set; }
    }
}