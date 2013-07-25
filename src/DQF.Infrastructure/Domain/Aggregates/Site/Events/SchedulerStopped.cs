using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Site.Events
{
    public class SchedulerStopped: Event
    {
        public bool Restart { get; set; }
    }
}