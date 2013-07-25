using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AKQ.Domain.Services;

namespace AKQ.Domain.Messaging
{
    public class Dispatcher
    {
        //private readonly SemaphoreSlim _semaphoreSlim;
        //private readonly Dictionary<Type, Action<Type, object>> _invokers = new Dictionary<Type, Action<Type, object>>();
        //private readonly Dictionary<Type, string> _queueNames = new Dictionary<Type, string>();
        private readonly Dictionary<string, EventsQueue> _queues = new Dictionary<string, EventsQueue>();

        private readonly List<IEventHandler> _asyncHandlers = new List<IEventHandler>();

        public Dispatcher(IEnumerable<IEventHandler> handlers)
        {
            foreach (var handler in handlers)
            {
                var type = handler.GetType();
                var attr = type.GetCustomAttribute<ProcessingOrderAttribute>();
                if (attr.IsAsync)
                {
                    _asyncHandlers.Add(handler);
                    continue;
                }
                EventsQueue messageQueue;
                if (!_queues.ContainsKey(attr.QueueName))
                {
                    messageQueue = new EventsQueue(attr.QueueName);
                    _queues.Add(attr.QueueName, messageQueue);
                }
                else
                {
                    messageQueue = _queues[attr.QueueName];
                }
                messageQueue.AddHandler(handler, attr.Order);
            }
        }

        public void Dispatch<T>(T message) where T : GameEvent
        {
            foreach (var memoryQueue in _queues)
            {
                memoryQueue.Value.Enqueue(message);
            }
            foreach (var asyncHandler in _asyncHandlers)
            {
                IEventHandler handler = asyncHandler;
                Task.Factory.StartNew(() => handler.Invoke(message));
            }
        }
    }
}