using CQRS.Core;
using CQRS.Core.Infrastructure;
using Loveboat.Domain.Messages.Commands;

namespace Loveboat.Domain.CommandHandlers
{
    public class ArrivalCommandHandler : ICommandHandler<ArrivalCommand>
    {
        private readonly IShipRepository repository;

        public ArrivalCommandHandler(IShipRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(ArrivalCommand message)
        {
            var aggregate = repository.GetById(message.ArrivingShipId);

            aggregate.HandleCommand(message);

            repository.Save(aggregate);
        }
    }
}