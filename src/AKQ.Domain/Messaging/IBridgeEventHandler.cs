using System;
using System.Collections.Generic;
using AKQ.Domain.Services;

namespace AKQ.Domain.Messaging
{
    public abstract class BridgeEventHandler: IEventHandler
    {
        private readonly Dictionary<Type, Action<object>> _handlers = new Dictionary<Type, Action<object>>();


        protected virtual BridgeEventHandler AddHandler<TMessage>(Action<TMessage> handler) where TMessage:GameEvent
        {
            _handlers.Add(typeof(TMessage), o =>
            {
                var message = (TMessage) o;
                handler(message);
            });
            return this;
        }

        public void Invoke(object message)
        {
            var type = message.GetType();
            if (_handlers.ContainsKey(type))
            {
                _handlers[message.GetType()](message);
            }
        }
    }
}