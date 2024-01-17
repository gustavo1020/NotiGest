using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Core.Contracts
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllEntityAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<T?> GetByIdAsync(Guid id);
        Task<bool> AddEntityAsync(T entity);
        Task<bool> UpdateEntityAsync(T entity);
        Task<bool> DeleteEntityAsync(T entity);
    }
}
