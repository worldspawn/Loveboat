using Loveboat.Domain.Aggregates.Ship;

namespace CQRS.Core.Infrastructure
{
    public interface IShipRepository : IRepository<ShipAggregate>
    {
    }
}