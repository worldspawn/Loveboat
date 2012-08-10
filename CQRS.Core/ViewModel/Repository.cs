using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CQRS.Core.ViewModel
{
    public class Repository<TType> : IRepository<TType>
        where TType : class, IPersistable
    {
        private readonly IRepositoryCollection<TType> _collection;

        public Repository(IRepositoryCollection<TType> collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            _collection = collection;
        }

        public virtual TType Single(Expression<Func<TType, bool>> criteria)
        {
            return _collection.AsQueryable()
                .SingleOrDefault(criteria);
        }

        public virtual TType Insert(TType entity)
        {
            if (entity.Id == default(Guid))
                entity.Id = Guid.NewGuid();

            _collection.Insert(entity);

            if (ItemInserted != null)
                ItemInserted(entity);

            return entity;
        }

        public virtual TType Delete(TType entity)
        {
            _collection.Delete(entity);
            if (ItemDeleted != null)
                ItemDeleted(entity);

            return entity;
        }

        private IQueryable<TType> ApplyWhere(IRepositoryCollection<TType> collection, Expression<Func<TType, bool>> criteria)
        {
            if (criteria != null)
                return collection.AsQueryable().Where(criteria);

            return collection.AsQueryable();
        }

        public virtual IEnumerable<TType> Find()
        {
            return Find(null);
        }

        public virtual IEnumerable<TType> Find(Expression<Func<TType, bool>> criteria)
        {
            return ApplyWhere(_collection, criteria);
        }

        public virtual IEnumerable<TType> Find(Expression<Func<TType, bool>> criteria,
                                               Expression<Func<TType, object>> orderBy, int skip = 0, int? take = null)
        {
            if (orderBy == null) throw new ArgumentNullException("orderBy");
            var result = (IQueryable<TType>)ApplyWhere(_collection, criteria).OrderBy(orderBy);
            if (skip > 0)
                result = result.Skip(skip);
            if (take.HasValue)
                result = result.Take(take.Value);

            return result;
        }

        public virtual bool Any(Expression<Func<TType, bool>> criteria)
        {
            return _collection.AsQueryable().Any(criteria);
        }

        public virtual int Count(Expression<Func<TType, bool>> criteria)
        {
            return _collection.AsQueryable().Count(criteria);
        }

        public virtual TType Update(TType entityToUpdate)
        {
            _collection.Update(entityToUpdate);
            if (ItemUpdated != null)
                ItemUpdated(entityToUpdate);
            
            return entityToUpdate;
        }

        public event Action<TType> ItemDeleted;
        public event Action<TType> ItemUpdated;
        public event Action<TType> ItemInserted;
    }
}