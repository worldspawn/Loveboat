using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonDomain.Core;
using Loveboat.Domain.Messages.Events;

namespace Loveboat.Domain.Aggregates.Ship
{
    public class ShipAggregate2 : AggregateBase
    {
        public ShipAggregate2()
        {
        }

        private ShipAggregate2(Guid id, string name, string location)
        {
            RaiseEvent(new ShipCreatedEvent(id, name, location));
        }

        public static ShipAggregate2 Create(Guid id, string name, string location)
        {
            return new ShipAggregate2(id, name, location);
        }
    }
}
