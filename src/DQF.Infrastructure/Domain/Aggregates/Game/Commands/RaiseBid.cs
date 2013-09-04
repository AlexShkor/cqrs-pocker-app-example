using System.Collections.Generic;
using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Commands
{
    public class RaiseBid : Command
    {
        public string UserId { get; set; }

        public long Amount { get; set; }
    }
}