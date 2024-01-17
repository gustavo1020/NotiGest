using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Infrastructure.Context;
using Core.Contracts;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly NotiGestDbContext notiGestDbContext;

        public BaseRepository(NotiGestDbContext notiGestDbContext)
        {
            this.notiGestDbContext = notiGestDbContext;
        }
        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await notiGestDbContext.Set<T>().FindAsync(id);
        }
        public async Task<IEnumerable<T>> GetAllEntityAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = notiGestDbContext.Set<T>().AsNoTracking();

            if (predicate is not null)
                query = query.Where(predicate);

            if (include != null)
                return await include(query).ToListAsync();

            return await query.ToListAsync();
        }

        public async Task<bool> AddEntityAsync(T entity)
        {
            await notiGestDbContext.Set<T>().AddAsync(entity);
            await notiGestDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateEntityAsync(T entity)
        {
            notiGestDbContext.Entry(entity).State = EntityState.Modified;
            await notiGestDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEntityAsync(T entity)
        {
            notiGestDbContext.Entry(entity).State = EntityState.Modified;
            await notiGestDbContext.SaveChangesAsync();
            return true;

        }

    }
}
