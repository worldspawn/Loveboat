using System;
using System.Linq;
using System.Web.Mvc;
using CQRS.Core;
using Loveboat.Domain.Messages.Commands;
using Loveboat.Domain.ViewModels;
using Loveboat.ViewModelCache;

namespace Loveboat.Controllers
{
    public class ShipsController : Controller
    {
        private readonly IViewModelCache viewModelCache;
        private readonly IBus bus;

        public ShipsController(/*IViewModelCache viewModelCache, */IBus bus)
        {
            //if (viewModelCache == null) throw new ArgumentNullException("viewModelCache");
            if (bus == null) throw new ArgumentNullException("bus");
            //this.viewModelCache = viewModelCache;
            this.bus = bus;
        }

        [HttpGet]
        public ViewResult Index()
        {
            /*var ships = viewModelCache.GetAll<ShipViewModel>();
            var model = new ShipsViewModel() {Ships = ships};

            var fake = (ShipViewModel)TempData["Fake"];
            if(fake!=null)
                model.Ships.First(s => s.Id == fake.Id).Location = fake.Location;

            return View("Index", model);*/
            var command = new ArrivalCommand() {ArrivalPort = "Port", ArrivingShipId = Guid.NewGuid()};
            bus.Send(command);

            var model = new ShipsViewModel() { Ships = Enumerable.Empty<ShipViewModel>() };
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Arrive(ArrivalCommand command)
        {
            if (!ModelState.IsValid)
                return Index();

            bus.Send(command);

            TempData["Fake"] = new ShipViewModel() {Id = command.ArrivingShipId, Location = command.ArrivalPort};

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Depart(DepartureCommand command)
        {
            if (!ModelState.IsValid)
                return Index();

            bus.Send(command);

            TempData["Fake"] = new ShipViewModel() { Id = command.DepartingShipId, Location = "At Sea" };

            return RedirectToAction("Index");
        }
    }
}