using System;
using System.Linq;
using System.Web.Mvc;
using CQRS.Core;
using CQRS.Core.ViewModel;
using Loveboat.Domain.Messages.Commands;
using Loveboat.Domain.ViewModels;
using Loveboat.ViewModelCache;

namespace Loveboat.Controllers
{
    public class ShipsController : Controller
    {
        private readonly IDtoRepository<ShipViewModel> _shipRepository;
        private readonly IBus bus;

        public ShipsController(IDtoRepository<ShipViewModel> shipRepository, IBus bus)
        {
            if (shipRepository == null) throw new ArgumentNullException("shipRepository");
            if (bus == null) throw new ArgumentNullException("bus");
            _shipRepository = shipRepository;
            this.bus = bus;
        }

        [HttpGet]
        public ActionResult Reset()
        {
            bus.Send(new ShipCreatedCommand("Melbourne"));
            bus.Send(new ShipCreatedCommand("Sydney"));
            bus.Send(new ShipCreatedCommand("Perth"));
            bus.Send(new ShipCreatedCommand("Hobart"));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Index()
        {
            var ships = _shipRepository.Find();
            var model = new ShipsViewModel() {Ships = ships};

            var fake = (ShipViewModel)TempData["Fake"];
            if(fake!=null)
                model.Ships.First(s => s.Id == fake.Id).Location = fake.Location;

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