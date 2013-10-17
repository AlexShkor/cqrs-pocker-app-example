using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Site.Commands
{
    public class StopScheduler:Command
    {
        public bool Restart { get; set; }
    }
}