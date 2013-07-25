using System.Collections.Generic;
using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Events
{
    public class GameCreated: Event
    {
        public List<string> Users { get; set; }
    }
}