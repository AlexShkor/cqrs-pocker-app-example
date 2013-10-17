using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Site.Events
{
    public class SchedulerStopped: Event
    {
        public bool Restart { get; set; }
    }
}