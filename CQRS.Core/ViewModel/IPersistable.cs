using System;

namespace CQRS.Core.ViewModel
{
    public interface IPersistable
    {
        Guid Id { get; set; }
    }
}