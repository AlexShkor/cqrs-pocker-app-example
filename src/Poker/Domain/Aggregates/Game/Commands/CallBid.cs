using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Game.Commands
{
    public class CallBid : Command
    {
        public string UserId { get; set; }
    }
}