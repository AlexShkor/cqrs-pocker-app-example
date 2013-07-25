using System.Collections.Generic;
using AKQ.Domain;
using PAQK.Domain.Aggregates.Game.Data;
using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Events
{
    public class GameCreated: Event
    {
        public List<UserPosition> Users { get; set; }
        public List<Card> Cards { get; set; }
        public string DealerUserId { get; set; }
    }
}