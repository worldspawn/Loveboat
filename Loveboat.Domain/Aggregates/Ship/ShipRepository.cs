using CQRS.Core;
using CQRS.Core.Infrastructure;
using EventStore;
using Loveboat.Domain.Infrastructure;

namespace Loveboat.Domain.Aggregates.Ship
{
    public class ShipRepository : Repository<ShipAggregate>, IShipRepository
    {
        public ShipRepository(IBus bus, IStoreEvents eventStore)
            : base(bus, eventStore)
        {
        }
    }
}
