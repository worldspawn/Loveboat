using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using CQRS.Core.Configuration;
using CQRS.Core.Infrastructure;
using CQRS.Core.Messaging;
using CQRS.Core.ViewModel;
using Loveboat.Domain.CommandHandlers;
using Loveboat.Domain.EventHandlers;
using Loveboat.Domain.Messages.Commands;
using Loveboat.Domain.Messages.Events;

namespace Loveboat
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Ships", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();

            string busEndPoint = ConfigurationManager.AppSettings["BusEndPointUri"];

            builder.RegisterModule(new MassTransitModule(busEndPoint));
            builder.RegisterModule(new EventStoreModule("loveboat.events"));
            builder.RegisterModule(new MongoModule("loveboat.dto.string", "loveboat.dto.databaseName"));
            builder.RegisterModule(new RepositoryModule(typeof (EventRepository<>),
                                                        typeof (IEventRepository<>)));
            builder.RegisterModule(new RepositoryModule(typeof (DtoRepository<>), typeof (IDtoRepository<>)));

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            var commandHost = new MessageHost(container);
            commandHost.RegisterMessageHandlers(
                new MessageRegistration<ArrivalCommand, ArrivalCommandHandler>(),
                new MessageRegistration<DepartureCommand, DepartureCommandHandler>(),
                new MessageRegistration<ShipCreatedCommand, ShipCreatedCommandHandler>()
                );

            var eventHost = new MessageHost(container);
            eventHost.RegisterMessageHandlers(
                new MessageRegistration<ArrivedEvent, ArrivalEventHandler>(),
                new MessageRegistration<DepartedEvent, DepartedEventHandler>(),
                new MessageRegistration<ShipCreatedEvent, ShipCreatedEventHandler>()
                );
        }
    }
}