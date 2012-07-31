using System;
using CQRS.Core;

namespace Loveboat.Domain.Configuration
{
    public interface IBus
    {
        void Send<T>(T command) where T : class, IMessage;
        void RegisterHandler<T>(Action<T> handler) where T : class, IMessage;
    }
}