using System;
using CQRS.Core.ViewModel;

namespace CQRS.Core
{
    public interface IViewModelUpdatedEvent<TDto> : IEvent
        where TDto : IDto
    {
        ViewModelUpdateType UpdateType { get; set; }
        TDto Dto { get; set; }
    }

    public class ViewModelUpdatedEvent<TDto> : IViewModelUpdatedEvent<TDto>
        where TDto : IDto
    {
        public Guid SourceId { get; set; }
        public ViewModelUpdateType UpdateType { get; set; }
        public TDto Dto { get; set; }
    }

    public interface IViewModelUpdatedEventHandler<TDto> : IEventHandler<ViewModelUpdatedEvent<TDto>>
        where TDto : IDto
    {
        
    }

    public class ViewModelUpdatedEventHandler<TDto> : IViewModelUpdatedEventHandler<TDto>
        where TDto : IDto
    {
        private readonly IViewModelEventDispatcher _viewModelEventDispatcher;

        public ViewModelUpdatedEventHandler(IViewModelEventDispatcher viewModelEventDispatcher)
        {
            _viewModelEventDispatcher = viewModelEventDispatcher;
        }

        public void Handle(ViewModelUpdatedEvent<TDto> message)
        {
            _viewModelEventDispatcher.Change(message.Dto, message.UpdateType);
        }
    }

    public interface IViewModelEventDispatcher
    {
       void Change<TDto>(TDto dto, ViewModelUpdateType viewModelUpdateType);
    }

    public enum ViewModelUpdateType
    {
        Insert,
        Update,
        Delete
    }
}