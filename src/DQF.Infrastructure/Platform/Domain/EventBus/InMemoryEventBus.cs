using System.Collections.Generic;
using PAQK.Platform.Domain.Interfaces;

namespace PAQK.Platform.Domain.EventBus
{
    public class InMemoryEventBus : IEventBus
    {
        public List<IEvent> Events = new List<IEvent>();

        public void Publish(IEvent eventMessage)
        {
            Events.Add(eventMessage);
        }

        public void Publish(IEnumerable<IEvent> eventMessages)
        {
            Events.AddRange(eventMessages);
        }
    }
}