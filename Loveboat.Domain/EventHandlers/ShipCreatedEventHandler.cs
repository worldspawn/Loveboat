using System.Linq;
using CQRS.Core;
using CQRS.Core.ViewModel;
using Loveboat.Domain.Messages.Events;
using Loveboat.Domain.ViewModels;

namespace Loveboat.Domain.EventHandlers
{
    public class ShipCreatedEventHandler: IEventHandler<ShipCreatedEvent>
    {
        private readonly IDtoRepository<ShipViewModel> _shipViewRepository;

        public ShipCreatedEventHandler(IDtoRepository<ShipViewModel> shipViewRepository)
        {
            _shipViewRepository = shipViewRepository;
        }

        public void Handle(ShipCreatedEvent @event)
        {
            var shipViewModel = _shipViewRepository.Find(x => x.Id == @event.ShipId).FirstOrDefault();
            
            if (shipViewModel != null)
                return;

            shipViewModel = new ShipViewModel { Id = @event.ShipId, Name = @event.Name, Location = @event.CurrentLocation };
            _shipViewRepository.Insert(shipViewModel);
        }
    }
}