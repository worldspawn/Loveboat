using System.Linq;
using FluentMongo.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace CQRS.Core.ViewModel
{
    public class MongoRepositoryCollection<TType> : IRepositoryCollection<TType>
        where TType : class, IPersistable
    {
        private readonly MongoCollection<TType> _collection;

        public MongoRepositoryCollection(MongoDatabase mongoDatabase)
        {
            var type = typeof (TType);
            if (!mongoDatabase.CollectionExists(type.Name))
                mongoDatabase.CreateCollection(type.Name);
            _collection = mongoDatabase.GetCollection<TType>(type.Name, SafeMode.True);
        }

        public IQueryable<TType> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public TType Insert(TType item)
        {
            _collection.Insert(item);
            return item;
        }

        public TType Delete(TType item)
        {
            IMongoQuery query = Query<TType>.EQ(e => e.Id, item.Id);
            _collection.Remove(query, RemoveFlags.Single);

            return item;
        }

        public TType Update(TType item)
        {
            IMongoQuery query = Query<TType>.EQ(e => e.Id, item.Id);
            IMongoUpdate update = Update<TType>.Replace(item);
            _collection.Update(query, update);

            return item;
        }
    }
}