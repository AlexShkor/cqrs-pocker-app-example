using System.Collections.Generic;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Game.Events
{
    public class CardsDealed : Event
    {
        public string GameId { get; set; }
        public List<PlayerCard> Cards { get; set; }
    }
}