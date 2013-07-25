using System.Collections.Generic;
using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Commands
{
    public class FoldBid : Command
    {
        public string GameId { get; set; }

        public string UserId { get; set; }
    }

    public class RaiseBid : Command
    {
        public string GameId { get; set; }

        public string UserId { get; set; }
    }
}