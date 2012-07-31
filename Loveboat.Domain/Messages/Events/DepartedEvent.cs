using System;
using CQRS.Core;
using Loveboat.Domain.Messages.Commands;

namespace Loveboat.Domain.Messages.Events
{
    public class DepartedEvent : IEvent
    {
        public DepartedEvent(Guid id)
        {
            Id = id;
        }

        public DepartedEvent(DepartureCommand command)
        {
            Id = command.DepartingShipId;
        }

        public Guid Id { get; set; }
    }
}