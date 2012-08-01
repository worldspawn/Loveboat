using System.Configuration;
using Autofac;
using MongoDB.Driver;

namespace CQRS.Core.Configuration
{
    public class MongoModule : Module
    {
        private readonly string _connectionString;

        public MongoModule(string connectionString)
        {
            _connectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((context) =>
                                 {
                                     return MongoDatabase.Create(_connectionString);
                                 }).SingleInstance();

            base.Load(builder);
        }
    }
}