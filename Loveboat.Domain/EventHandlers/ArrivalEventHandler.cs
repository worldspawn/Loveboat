using CQRS.Core;
using CQRS.Core.ViewModel;
using Loveboat.Domain.Messages.Events;
using Loveboat.Domain.ViewModels;

namespace Loveboat.Domain.EventHandlers
{
    public class ArrivalEventHandler: IEventHandler<ArrivedEvent>
    {
        private readonly IDtoRepository<ShipViewModel> _shipViewRepository;

        public ArrivalEventHandler(IDtoRepository<ShipViewModel> shipViewRepository)
        {
            _shipViewRepository = shipViewRepository;
        }

        public void Handle(ArrivedEvent @event)
        {
            var shipViewModel = _shipViewRepository.ById(@event.Id);
            
            if (shipViewModel == null) return;

            shipViewModel.Location = @event.ArrivalPort;
            _shipViewRepository.Update(shipViewModel);
        }
    }
}