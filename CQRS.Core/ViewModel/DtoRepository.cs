using System.Collections.Generic;
using System.Linq.Expressions;

namespace CQRS.Core.ViewModel
{
    public class DtoRepository<TDto> : Repository<TDto>
        where TDto : class, IPersistedDto
    {
        

        public DtoRepository(IRepositoryCollection<TDto> collection) : 
            base(collection)
        {
        }
    }
}