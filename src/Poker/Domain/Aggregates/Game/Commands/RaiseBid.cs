using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Game.Commands
{
    public class RaiseBid : Command
    {
        public string UserId { get; set; }

        public long Amount { get; set; }
    }
}