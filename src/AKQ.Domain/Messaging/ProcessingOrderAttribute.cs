using System;

namespace AKQ.Domain.Messaging
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
    public class ProcessingOrderAttribute: Attribute
    {
        public string QueueName { get; set; }
        public int Order { get; set; }
        public bool IsAsync { get; set; }

        public ProcessingOrderAttribute()
        {
            
        }

        public ProcessingOrderAttribute(QueueNamesEnum queueName)
        {
            QueueName = queueName.ToString();
        }

        public ProcessingOrderAttribute(QueueNamesEnum queueName, int order):this(queueName)
        {
            Order = order;
        }
    }
}