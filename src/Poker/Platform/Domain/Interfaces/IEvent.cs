using Poker.Platform.Domain.Messages;

namespace Poker.Platform.Domain.Interfaces
{
    /// <summary>
    /// Domain Event
    /// </summary>
    public interface IEvent
    {
        string Id { get; set; }
        EventMetadata Metadata { get; set; }
    }
}