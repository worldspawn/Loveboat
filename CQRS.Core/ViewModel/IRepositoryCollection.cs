using System.Linq;

namespace CQRS.Core.ViewModel
{
    public interface IRepositoryCollection<TType>
        where TType : class, IPersistable
    {
        IQueryable<TType> AsQueryable();
        TType Insert(TType item);
        TType Delete(TType item);
        TType Update(TType item);
    }
}