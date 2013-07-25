using PAQK.Platform.Domain.Messages;

namespace PAQK.Platform.Domain.Interfaces
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