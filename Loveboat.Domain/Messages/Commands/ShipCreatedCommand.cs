using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Core;

namespace Loveboat.Domain.Messages.Commands
{
    public class ShipCreatedCommand : ICommand
    {
        private readonly string _currentLocation;

        public ShipCreatedCommand(string currentLocation)
        {
            _currentLocation = currentLocation;
        }

        public string CurrentLocation
        {
            get { return _currentLocation; }
        }
    }
}
