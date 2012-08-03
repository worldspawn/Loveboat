using System;

namespace CQRS.Core.Configuration
{
    public class EasyNetBus : CQRS.Core.IBus
    {
        private readonly EasyNetQ.IBus _bus;

        public EasyNetBus(EasyNetQ.IBus bus)
        {
            _bus = bus;
        }

        public void PublishEvent(object @event)
        {
            using (var publishChannel = _bus.OpenPublishChannel())
            {
                publishChannel.Publish(@event);
            }
        }

        public void Send<T>(T command) where T : class, IMessage
        {
            using (var publishChannel = _bus.OpenPublishChannel())
            {
                publishChannel.Publish(command);
            }
        }

        public void RegisterHandler<T>(Action<T> handler) where T : class, IMessage
        {
            _bus.Subscribe(Guid.NewGuid().ToString(), handler);
        }
    }
}