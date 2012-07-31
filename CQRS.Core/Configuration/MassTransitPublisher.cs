using System;
using EventStore;
using EventStore.Dispatcher;
using Magnum.Reflection;
using MassTransit;

namespace CQRS.Core.Configuration
{
    public class MassTransitPublisher : IBus, IDispatchCommits
    {
        private readonly IServiceBus _bus;

        public MassTransitPublisher(IServiceBus bus)
        {
            _bus = bus;
        }

        #region IBus Members

        void IBus.Send<T>(T command)
        {
            _bus.Publish(command);
        }

        void IBus.RegisterHandler<T>(Action<T> handler)
        {
            _bus.SubscribeHandler(handler);
        }

        #endregion

        #region IDispatchCommits Members

        void IDispatchCommits.Dispatch(Commit commit)
        {
            commit.Events.ForEach(@event => { this.FastInvoke("PublishEvent", @event.Body); });
        }

        public void Dispose()
        {
            _bus.Dispose();
        }

        #endregion
    }
}