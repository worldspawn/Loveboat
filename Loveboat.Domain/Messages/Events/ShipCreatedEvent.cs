using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Core;

namespace Loveboat.Domain.Messages.Events
{
    public class ShipCreatedEvent : IEvent
    {
        private readonly Guid _shipId;
        private readonly string _currentLocation;

        public ShipCreatedEvent(Guid shipId, string currentLocation)
        {
            _shipId = shipId;
            _currentLocation = currentLocation;
        }

        public Guid ShipId
        {
            get { return _shipId; }
        }

        public string CurrentLocation
        {
            get { return _currentLocation; }
        }
    }
}
