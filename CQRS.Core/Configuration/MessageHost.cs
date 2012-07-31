using System;
using System.Linq;
using Autofac;

namespace CQRS.Core.Configuration
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

    public abstract class MessageRegistration
    {
        public abstract void Apply(ContainerBuilder builder, IContainer container, IBus bus);
    }

    public class MessageRegistration<TMessage, TMessageHandler> : MessageRegistration
        where TMessageHandler : IMessageHandler<TMessage>
        where TMessage : class, IMessage
    {
        public override void Apply(ContainerBuilder builder, IContainer container, IBus bus)
        {
            builder.RegisterType<TMessageHandler>();

            bus.RegisterHandler<TMessage>(msg =>
                                              {
                                                  var handler = container.Resolve<TMessageHandler>();
                                                  handler.Handle(msg);
                                              });
        }
    }
}