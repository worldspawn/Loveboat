using System;
using System.Linq;
using Autofac;

namespace CQRS.Core.Messaging
{
    public class MessageHost
    {
        private readonly IBus _bus;
        private readonly IContainer _container;

        public MessageHost(IContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            _bus = container.Resolve<IBus>();
            _container = container;
        }

        public void RegisterMessageHandlers(params MessageRegistration[] registrations)
        {
            if (registrations == null || !registrations.Any())
                return;

            var builder = new ContainerBuilder();
            for (int i = 0; i < registrations.Length; i++)
                registrations[i].Apply(builder, _container, _bus);
            builder.Update(_container);
        }

        public void RegisterMessageHandler<TMessage, TMessageHandler>()
            where TMessageHandler : IMessageHandler<TMessage>
            where TMessage : class, IMessage
        {
            var builder = new ContainerBuilder();
            new MessageRegistration<TMessage, TMessageHandler>().Apply(builder, _container, _bus);
            builder.Update(_container);
        }
    }
}