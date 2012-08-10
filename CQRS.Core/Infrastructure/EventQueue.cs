using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Core.Infrastructure
{
    public class EventQueue
    {
        public void QueueEventForExecution(IEvent @event)
        {
            var message = new Message()
            {
                AggregateId = @event.AggregateId,
                Inserted = DateTime.UtcNow,
                Version = @event.Version,
                Payload = @event
            };

            var sequence = new Sequence()
            {
                AggregateId = @event.AggregateId,
                Cleared = new DateTime(2000, 1, 1),
                MinVersion = 1,
                MaxVersion = 1,
                Messages = 0
            };


        }
    }
}