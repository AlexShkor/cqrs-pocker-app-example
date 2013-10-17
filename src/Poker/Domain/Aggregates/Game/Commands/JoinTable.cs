using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Game.Commands
{
    public class JoinTable: Command
    {
        public string UserId { get; set; }

        public long Cash { get; set; }
    }
}