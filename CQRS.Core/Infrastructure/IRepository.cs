using System;
using CQRS.Core.Aggregates;

namespace CQRS.Core.Infrastructure
{
    public interface IEventRepository<T> where T : AggregateBase<IEvent>
    {
        T GetById(Guid id);
        void Create(T aggregate);
        void Update(T aggregate);
    }
}