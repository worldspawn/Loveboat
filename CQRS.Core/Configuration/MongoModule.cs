using System.Configuration;
using Autofac;
using MongoDB.Driver;

namespace CQRS.Core.Configuration
{
    public class MongoModule : Module
    {
        private readonly string _connectionString;
        private readonly string _databaseName;

        public MongoModule(string connectionString, string databaseName)
        {
            _connectionString = connectionString;
            _databaseName = databaseName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((context) => { return MongoServer.Create(ConfigurationManager.ConnectionStrings[_connectionString].ConnectionString); }).SingleInstance();

            builder.Register((context) =>
                                 {
                                     var mongoServer = context.Resolve<MongoServer>();
                                     return mongoServer.GetDatabase(ConfigurationManager.AppSettings[_databaseName]);
                                 }).SingleInstance();

            base.Load(builder);
        }
    }
}