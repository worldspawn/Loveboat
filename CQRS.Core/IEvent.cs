using System;

namespace CQRS.Core
{
    public interface IEvent : IMessage
    {
        //Guid Id { get; set; }
    }
}