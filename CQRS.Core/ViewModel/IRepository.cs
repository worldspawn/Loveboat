using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CQRS.Core.ViewModel
{
    public interface IRepository<TType>
    {
        bool Any(Expression<Func<TType, bool>> criteria);
        TType Single(Expression<Func<TType, bool>> criteria);
        int Count(Expression<Func<TType, bool>> criteria);
        TType Delete(TType entity);
        IEnumerable<TType> Find();
        IEnumerable<TType> Find(Expression<Func<TType, bool>> criteria);

        IEnumerable<TType> Find(Expression<Func<TType, bool>> criteria, Expression<Func<TType, object>> orderBy,
                                int skip = 0, int? take = null);

        TType Insert(TType entity);
        TType Update(TType entityToUpdate);

        event Action<TType> ItemDeleted;
        event Action<TType> ItemUpdated;
        event Action<TType> ItemInserted;
    }
}