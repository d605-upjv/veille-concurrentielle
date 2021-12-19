﻿using System.Linq.Expressions;

namespace VeilleConcurrentielle.Infrastructure.Data
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> GetByIdAsync(string id);
        IAsyncEnumerable<T> GetAllAsync();
        IAsyncEnumerable<T> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task InsertAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
