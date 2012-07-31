using System;
using CQRS.Core.ViewModel;

namespace Loveboat.Domain.ViewModels
{
    public class ShipViewModel : IPersistedDto
    {
        public Guid Id { get; set; }

        public string Location { get; set; }

        public string Name { get; set; }
    }
}