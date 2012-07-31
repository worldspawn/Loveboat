using System;
using System.Configuration;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Loveboat.Domain.CommandHandlers;
using Loveboat.Domain.Configuration;
using Loveboat.Domain.Messages.Commands;
using Loveboat.Messages;

namespace Loveboat
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
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
                new { controller = "Ships", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();

            var busEndPoint = ConfigurationManager.AppSettings["BusEndPointUri"];

            builder.RegisterModule(new MassTransitModule(busEndPoint));
            builder.RegisterModule<EventStoreModule>();

            builder.RegisterType<ArrivalCommandHandler>();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            var bus = container.Resolve<IBus>();

            RegisterMessageHandler<ArrivalCommand, ArrivalCommandHandler>(bus, container);
        }

        void RegisterMessageHandler<TMessage, TMessageHandler>(IBus bus, IContainer container) 
            where TMessageHandler : IMessageHandler<TMessage>
            where TMessage : class, IMessage
        {
            bus.RegisterHandler<TMessage>(msg =>
                                              {
                                                  var handler = container.Resolve<TMessageHandler>();
                                                  handler.Handle(msg);
                                              });
        }
    }
}