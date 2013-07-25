namespace AKQ.Domain.Messaging
{
    public interface IEventHandler
    {
        void Invoke(object message);
    }
}