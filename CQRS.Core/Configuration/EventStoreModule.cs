using System;
using Autofac;
using EventStore;
using EventStore.Dispatcher;
using EventStore.Serialization;

namespace CQRS.Core.Configuration
{
    public class EventStoreModule : Module
    {
        private static readonly byte[] EncryptionKey = new byte[]
                                                           {
                                                               0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa,
                                                               0xb, 0xc, 0xd, 0xe, 0xf
                                                           };

        private readonly AuthorizationPipelineHook _authorizationPipelineHook;

        private readonly string _connectionName;
        private readonly byte[] _encryptionKey;

        public EventStoreModule(string connectionName, byte[] encryptionKey = null)
        {
            _connectionName = connectionName;
            _encryptionKey = encryptionKey;
            _authorizationPipelineHook = new AuthorizationPipelineHook();
            if (connectionName == null) throw new ArgumentNullException("connectionName");
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
                                 {
                                     return Wireup.Init()
                                         .LogToOutputWindow()
                                         .UsingMongoPersistence(_connectionName, new DocumentObjectSerializer())
                                         .InitializeStorageEngine()
                                         .UsingJsonSerialization()
                                         .Compress()
                                         .EncryptWith(_encryptionKey ?? EncryptionKey)
                                         .HookIntoPipelineUsing(new[] {_authorizationPipelineHook})
                                         .UsingAsynchronousDispatchScheduler(context.Resolve<IDispatchCommits>())
                                         .Build();
                                 }).As<IStoreEvents>();

            base.Load(builder);
        }
    }
}