using System;

namespace CQRS.Core
{
    public interface IBus
    {
        void Send<T>(T command) where T : class, IMessage;
        void RegisterHandler<T>(Action<T> handler) where T : class, IMessage;
    }
}