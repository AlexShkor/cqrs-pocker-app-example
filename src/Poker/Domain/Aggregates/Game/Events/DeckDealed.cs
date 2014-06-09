using System.Collections.Generic;
using Poker.Domain.Data;
using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Game
{
    public class DeckDealed : Event
    {
        public string GameId { get; set; }
        public List<Card> Cards { get; set; }
    }
}