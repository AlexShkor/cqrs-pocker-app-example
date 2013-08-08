using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Events
{
    public class TableCreated: Event
    {
        public string TableId { get; set; }
    }
}