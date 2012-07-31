using System;
using System.Collections.Generic;
using CQRS.Core;
using CQRS.Core.Aggregates;
using Loveboat.Domain.Messages.Commands;
using Loveboat.Domain.Messages.Events;

namespace Loveboat.Domain.Aggregates.Ship
{
    public class ShipAggregate : AggregateBase
    {
        public ShipAggregate()
        {
            UncommitedEvents = new List<IEvent>();
        }

        protected string CurrentLocation;

        public void HandleCommand(DepartureCommand command)
        {
            if (CurrentLocation == "At Sea")
                throw new ApplicationException();

            ApplyChange(new DepartedEvent(command));
        }

        public void HandleCommand(ArrivalCommand command)
        {
            if (CurrentLocation != "At Sea")
                throw new ApplicationException();

            ApplyChange(new ArrivedEvent(command));
        }

        public void HandleEvent(ArrivedEvent arrivedEvent)
        {
            CurrentLocation = arrivedEvent.ArrivalPort;
        }

        public void HandleEvent(DepartedEvent departedEvent)
        {
            CurrentLocation = "At Sea";
        }      
    }
}