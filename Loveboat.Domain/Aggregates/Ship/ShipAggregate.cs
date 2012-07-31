using System;
using System.Collections.Generic;
using CQRS.Core;
using CQRS.Core.Aggregates;
using Loveboat.Domain.Messages.Commands;
using Loveboat.Domain.Messages.Events;

namespace Loveboat.Domain.Aggregates.Ship
{
    public class ShipAggregate : AggregateBase<IEvent>
    {
        public ShipAggregate()
        {
            UncommitedEvents = new List<IEvent>();
            MapEvent<DepartedEvent>(OnDeparted);
        }

        private ShipAggregate(string currentLocation)
        {
            ApplyChange(new ShipCreatedEvent(Id, currentLocation));
        }

        protected string CurrentLocation;

        public static ShipAggregate Create(string currentLocation)
        {
            return new ShipAggregate(currentLocation);
        }

        public void Depart()
        {
            if (CurrentLocation == "At Sea")
                throw new ApplicationException();

            ApplyChange(new DepartedEvent(Id, "At Sea"));
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

        public void OnDeparted(DepartedEvent departedEvent)
        {
            CurrentLocation = departedEvent.CurrentLocation;
        }
    }
}