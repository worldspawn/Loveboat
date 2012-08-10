using CQRS.Core;
using CQRS.Core.Infrastructure;
using Loveboat.Domain.Aggregates.Ship;
using Loveboat.Domain.Messages.Commands;
using Magnum;

namespace Loveboat.Domain.CommandHandlers
{
    public class ShipCreatedCommandHandler : ICommandHandler<ShipCreatedCommand>
    {
        private readonly CommonDomain.Persistence.IRepository _eventRepository;

        public ShipCreatedCommandHandler(CommonDomain.Persistence.IRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        #region ICommandHandler<DepartureCommand> Members

        public void Handle(ShipCreatedCommand message)
        {
            ShipAggregate2 aggregate = ShipAggregate2.Create(message.AggregateId, message.Name, message.CurrentLocation);
            
            _eventRepository.Save(aggregate, CombGuid.Generate(), null);
        }

        #endregion
    }
}