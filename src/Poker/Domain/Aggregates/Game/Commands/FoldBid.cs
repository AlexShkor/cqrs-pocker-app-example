using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Game.Commands
{
    public class FoldBid : Command
    {
        public string UserId { get; set; }
    }
}