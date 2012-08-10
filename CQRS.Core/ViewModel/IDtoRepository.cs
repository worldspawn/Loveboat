using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CQRS.Core.ViewModel
{
    public interface IDtoRepository<TDto> : IRepository<TDto>
        where TDto : IPersistedDto
    {
        
    }
}