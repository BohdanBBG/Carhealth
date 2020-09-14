using CarHealth.Api.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHealth.ApiTest.TestRepositories
{
    public interface IIdentityMongoRepository<TUser> where TUser: User
    {
        public Task AddAsync(TUser user);


    }
}
