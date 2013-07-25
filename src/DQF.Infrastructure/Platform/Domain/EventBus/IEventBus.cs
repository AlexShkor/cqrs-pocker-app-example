using System.Collections.Generic;
using PAQK.Platform.Domain.Interfaces;

namespace PAQK.Platform.Domain.EventBus
{
    public interface IEventBus
    {
        void Publish(IEvent eventMessage);
        void Publish(IEnumerable<IEvent> eventMessages);
    }
}