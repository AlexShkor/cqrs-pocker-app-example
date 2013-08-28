using System.Collections.Generic;
using PAQK.Domain.Aggregates.Game.Data;
using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Events
{
    public class CardsDealed : Event
    {
        public string GameId { get; set; }
        public List<PlayerCard> Cards { get; set; }
    }
}