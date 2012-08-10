using System;

namespace CQRS.Core.ViewModel
{
    public static class RepositoryExtensions
    {
        public static void RegisterForViewUpdateEvents<TType>(this IRepository<TType> repository, IBus bus, Guid sourceId)
            where TType : IDto
        {
            repository.ItemDeleted += type => bus.Send(new ViewModelUpdatedEvent<TType> { Dto = type, UpdateType = ViewModelUpdateType.Insert, SourceId = sourceId });
            repository.ItemInserted += type => bus.Send(new ViewModelUpdatedEvent<TType> { Dto = type, UpdateType = ViewModelUpdateType.Insert, SourceId = sourceId });
            repository.ItemUpdated += type => bus.Send(new ViewModelUpdatedEvent<TType> { Dto = type, UpdateType = ViewModelUpdateType.Update, SourceId = sourceId });
        }
    }
}