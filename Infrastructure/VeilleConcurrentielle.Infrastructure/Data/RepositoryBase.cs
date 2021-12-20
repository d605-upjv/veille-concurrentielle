using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace VeilleConcurrentielle.Infrastructure.Data
{
    public class RepositoryBase<T> : IRepository<T> where T : EntityBase
    {
        private readonly DbContext _dbContext;
        public RepositoryBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public virtual IAsyncEnumerable<T> GetAllAsync()
        {
            return _dbContext.Set<T>().AsAsyncEnumerable();
        }
        public virtual IAsyncEnumerable<T> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>()
                   .Where(predicate)
                   .AsAsyncEnumerable();
        }
        public virtual void ComputeNewIdBeforeInsert(T entity)
        {
            entity.Id = Guid.NewGuid().ToString();
        }
        public async Task InsertAsync(T entity)
        {
            ComputeNewIdBeforeInsert(entity);
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
