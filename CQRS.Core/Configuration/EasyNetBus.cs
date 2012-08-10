using System;
using System.Collections.Generic;
using EasyNetQ;
using EventStore;
using EventStore.Dispatcher;
using Magnum.Reflection;
using EasyNetQ;

namespace CQRS.Core.Configuration
{
    public class DispatchResponse
    {
        public DispatchResponse()
        {
            Exceptions = new List<Exception>();
        }

        public bool Success
        {
            get { return Exceptions.Count == 0; }
        }

        private List<Exception> Exceptions { get; set; }
    }

    public class EasyNetBus : IBus, IDispatchCommits
    {
        private readonly EasyNetQ.IBus _bus;

        public EasyNetBus(EasyNetQ.IBus bus)
        {
            _bus = bus;
        }

        #region IBus Members

        public void PublishEvent(object @event)
        {
            using (IPublishChannel publishChannel = _bus.OpenPublishChannel())
            {
                publishChannel.FastInvoke(new[] {@event.GetType()}, "Publish", @event);
            }
        }

        public void Send<T>(T command) where T : class, IMessage
        {
            using (IPublishChannel publishChannel = _bus.OpenPublishChannel())
            {
                publishChannel.FastInvoke(new[] {command.GetType()}, "Publish", command);
            }
        }

        public void RegisterHandler<T>(Action<T> handler) where T : class, IMessage
        {
            _bus.Subscribe(Guid.NewGuid().ToString(), handler);
        }

        #endregion

        #region IDispatchCommits Members

        public void Dispose()
        {
        }

        void IDispatchCommits.Dispatch(Commit commit)
        {
            commit.Events.ForEach(@event => PublishEvent(@event.Body));
        }

        #endregion
    }
}