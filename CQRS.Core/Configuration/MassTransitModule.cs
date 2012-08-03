using Autofac;
using EventStore.Dispatcher;
using MassTransit;

namespace CQRS.Core.Configuration
{
    public class MassTransitModule : Module
    {
        private readonly string _endPoint;

        public MassTransitModule(string endPoint)
        {
            _endPoint = endPoint;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var endPoint = _endPoint.Replace("amqp://", "rabbitmq://");
            builder.Register(context =>
                                 {
                                     return ServiceBusFactory.New(x =>
                                                                      {
                                                                          x.ReceiveFrom(endPoint);
                                                                          x.UseRabbitMq();
                                                                          x.UseRabbitMqRouting();
                                                                          x.Subscribe(
                                                                              c =>
                                                                              c.LoadFrom(
                                                                                  context.Resolve<ILifetimeScope>()));
                                                                      });
                                 }).As<IServiceBus>().SingleInstance();

            builder.Register(context => { return new MassTransitPublisher(context.Resolve<IServiceBus>()); }).As
                <IBus, IDispatchCommits>().SingleInstance();

            base.Load(builder);
        }
    }
}