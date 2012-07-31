using CQRS.Core.Infrastructure;
using Loveboat.Domain.Messages.Commands;

namespace Loveboat.Domain.CommandHandlers
{
    public class DepartureCommandHandler //: IHandleMessages<DepartureCommand>
    {
        private readonly IShipRepository repository;

        public DepartureCommandHandler(IShipRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(DepartureCommand message)
        {
            var aggregate = repository.GetById(message.DepartingShipId);

            aggregate.HandleCommand(message);

            repository.Save(aggregate);
        }
    }
}