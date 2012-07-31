using System;
using System.Collections.Generic;
using System.Linq;
using CQRS.Core;
using CQRS.Core.Aggregates;
using CQRS.Core.Infrastructure;
using EventStore;

namespace Loveboat.Domain.Infrastructure
{
    public abstract class Repository<T> : IRepository<T> where T : AggregateBase, new()
    {
        private readonly IBus _bus;
        private readonly IStoreEvents _eventStore;

        protected Repository(IBus bus, IStoreEvents eventStore)
        {
            if (bus == null) throw new ArgumentNullException("bus");
            if (eventStore == null) throw new ArgumentNullException("eventStore");
            _bus = bus;
            _eventStore = eventStore;
        }

        #region IRepository<T> Members

        public T GetById(Guid id)
        {
            var aggregate = new T();
            IEnumerable<IEvent> eventsForAggreate = GetEventsFor(id);
            aggregate.LoadFromEvents(id, eventsForAggreate);
            return aggregate;
        }

        public void Save(T aggregate)
        {
            using (IEventStream stream = _eventStore.CreateStream(aggregate.Id))
            {
                foreach (IEvent uncommitedEvent in aggregate.UncommitedEvents)
                {
                    stream.Add(new EventMessage {Body = uncommitedEvent});

                    _bus.Send(uncommitedEvent);
                }

                stream.CommitChanges(Guid.NewGuid());
            }
        }

        #endregion

        private IEnumerable<IEvent> GetEventsFor(Guid id)
        {
            using (IEventStream stream = _eventStore.OpenStream(id, 0, int.MaxValue))
            {
                return stream.CommittedEvents.Select(e => e.Body as IEvent);
            }
        }
    }
}