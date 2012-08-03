using System;
using Autofac;
using EasyNetQ;

namespace CQRS.Core.Configuration
{
    public class EasyNetQModule : Module
    {
        private readonly string _connectionString;

        public EasyNetQModule(string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException("connectionString");
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context => RabbitHutch.CreateBus(_connectionString));
            builder.RegisterType<EasyNetBus>().AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}