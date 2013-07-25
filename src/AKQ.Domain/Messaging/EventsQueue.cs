using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKQ.Domain.Services;
using System.Threading.Tasks.Dataflow;
using NLog;
using Newtonsoft.Json;

namespace AKQ.Domain.Messaging
{
    public class EventsQueue: IMessageQueue
    {
        private readonly SortedList<int, IEventHandler> _handlers = new SortedList<int, IEventHandler>();
        private readonly ActionBlock<object> _messages;

        private Logger Logger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }

        private readonly string _queueName;

        public EventsQueue(string queueName)
        {
            _queueName = queueName;
            _messages = new ActionBlock<object>(message => InvokeHandlers(message));
        }

        public string QueueName
        {
            get { return _queueName; }
        }

        public void AddHandler(IEventHandler handler, int order)
        {
            _handlers.Add(order,handler);
        }

        public void Enqueue(GameEvent message)
        {
            _messages.Post(message);
        }

        private void InvokeHandlers(Object message)
        {
            foreach (var handler in _handlers)
            {
                MakeInvokeAttempts(handler.Value,message);
            }
        }

        private const int AttemptsCount = 3;
        private void MakeInvokeAttempts(IEventHandler handler, Object message)
        {
            for (int i = 0; i < AttemptsCount; i++)
            {
                try
                {
                    handler.Invoke(message);
                    return;
                }
                catch (NotImplementedException)
                {
                    return;
                }
                catch (Exception e)
                {
                    Logger.ErrorException(String.Format("Error when executing handler {0} with message {2}: {1}",
                                                        handler.GetType().Name,
                                                        JsonConvert.SerializeObject(message),
                                                        message.GetType().Name),
                                          e);
                }
            }
        }
    }
}