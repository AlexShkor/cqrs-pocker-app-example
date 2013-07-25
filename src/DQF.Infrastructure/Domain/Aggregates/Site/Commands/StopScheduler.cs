using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Site.Commands
{
    public class StopScheduler:Command
    {
        public bool Restart { get; set; }
    }
}