using System;
using CQRS.Core;

namespace Loveboat.Domain.Messages.Events
{
    public class DepartedEvent : IEvent
    {
        private readonly Guid _shipId;
        private readonly string _currentLocation;

        public DepartedEvent(Guid shipId, string currentLocation)
        {
            _shipId = shipId;
            _currentLocation = currentLocation;
        }

        public string CurrentLocation
        {
            get { return _currentLocation; }
        }

        public Guid ShipId
        {
            get { return _shipId; }
        }
    }
}