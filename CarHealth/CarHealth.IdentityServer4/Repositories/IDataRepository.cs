using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CarHealth.IdentityServer4.Repositories
{
    public interface IDataRepository
    {
        Task<bool> IsCollectionExistAsync<T>() where T : class;

        Task<IEnumerable<T>> GetAllAsync<T>() where T : class;
        Task<IEnumerable<T>> GetAllAsync<T>(int offser, int limit) where T : class;

        Task<IEnumerable<T>> GetManyASync<T> (Expression<Func<T,bool>> expression) where T : class;
        Task<IEnumerable<T>> GetManyASync<T>(Expression<Func<T, bool>> expression, int offset, int limit) where T : class;

        Task<T> GetOneAsync<T>(Expression<Func<T, bool>> expression) where T : class;

        Task AddAsync<T>(T entity) where T : class;
        Task AddMany<T>(IEnumerable<T> entities) where T : class;

        Task UpdateAsync<T>(T entity) where T : class;
        Task UpdateManyAsync<T>(IEnumerable<T> entities) where T : class;

        Task DeleteAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        Task DeleteAllAsync<T>() where T : class;

        Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task<long> CountAllAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

    }
}
