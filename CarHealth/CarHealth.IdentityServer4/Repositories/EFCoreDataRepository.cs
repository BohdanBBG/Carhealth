using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CarHealth.IdentityServer4.Repositories
{
    public class EFCoreDataRepository : IDataRepository
    {
        public EFCoreDataRepository()
        {

        }

        public Task AddAsync<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public Task AddMany<T>(IEnumerable<T> entities) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAllAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            throw new NotImplementedException();
        }

        public Task DeleteAllAsync<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync<T>(Expression<Func<T, bool>> expression) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync<T>(int offser, int limit) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetManyASync<T>(Expression<Func<T, bool>> expression) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetManyASync<T>(Expression<Func<T, bool>> expression, int offset, int limit) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> GetOneAsync<T>(Expression<Func<T, bool>> expression) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsCollectionExistAsync<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public Task UpdateManyAsync<T>(IEnumerable<T> entities) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
