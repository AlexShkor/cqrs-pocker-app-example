using AKQ.Domain.Services;

namespace AKQ.Domain.Messaging
{
    public interface IMessageQueue
    {
        void Enqueue(GameEvent message);
    }
}