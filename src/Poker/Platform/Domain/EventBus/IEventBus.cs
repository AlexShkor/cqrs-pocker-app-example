using System.Collections.Generic;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Platform.Domain.EventBus
{
    public interface IEventBus
    {
        void Publish(IEvent eventMessage);
        void Publish(IEnumerable<IEvent> eventMessages);
    }
}