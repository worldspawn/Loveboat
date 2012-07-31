using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using CQRS.Core.Infrastructure;
using Module = Autofac.Module;

namespace CQRS.Core.Configuration
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type repositoryType = typeof (IRepository<>);
            IEnumerable<Type> repositories = from x in assemblies.SelectMany(a => a.GetTypes())
                                             from z in x.GetInterfaces()
                                             let y = x.BaseType
                                             where
                                                 (y != null && y.IsGenericType &&
                                                  repositoryType.IsAssignableFrom(y.GetGenericTypeDefinition())) ||
                                                 (z.IsGenericType &&
                                                  repositoryType.IsAssignableFrom(z.GetGenericTypeDefinition()))
                                             select x;

            builder.RegisterAssemblyTypes(assemblies)
                .Where(repositories.Contains)
                .AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}