using System.Collections.Generic;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Data;
using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Game.Events
{
    public class GameCreated: Event
    {
        public List<Card> Cards { get; set; }
        public string GameId { get; set; }
        public List<TablePlayer> Players { get; set; }
    }
}