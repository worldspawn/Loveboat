using CQRS.Core;
using CQRS.Core.ViewModel;
using Loveboat.Domain.Messages.Events;
using Loveboat.Domain.ViewModels;

namespace Loveboat.Domain.EventHandlers
{
    public class DepartedEventHandler : IEventHandler<DepartedEvent>
    {
        private readonly IDtoRepository<ShipViewModel> _shipViewRepository;

        public DepartedEventHandler(IDtoRepository<ShipViewModel> shipViewRepository)
        {
            _shipViewRepository = shipViewRepository;
        }

        public void Handle(DepartedEvent @event)
        {
            var shipViewModel = _shipViewRepository.Single(x => x.Id == @event.ShipId);
            
            if (shipViewModel == null) return;

            shipViewModel.Location = "At Sea";
            _shipViewRepository.Update(shipViewModel);
        }
    }
}