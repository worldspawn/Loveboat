using System;
using CQRS.Core.Aggregates;

namespace CQRS.Core.Infrastructure
{
    public interface IRepository<T> where T : AggregateBase
    {
        T GetById(Guid id);
        void Save(T aggregate);
    }
}