using CarHealth.Seed.Models;
using CarHealth.Seed.Models.IdentityModels;
using CarHealth.Seed.Models.IdentityServer4Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Repositories
{
    public interface IIdentitySeedRepository
    {
        public bool IsClientCollectionEmpty();
        public bool IsIdentityResourceCollectionEmpty();
        public bool IsApiResourceCollectionEmpty();
        public bool IsUserExist(string email);
        public bool IsRoleExist(string name);

        public Task<bool> AddClientAsync(ClientEntity client);
        public Task<bool> AddIdentityResourceAsync(IdentityResourceEntity identityResource );
        public Task<bool> AddApiResourceAsync(ApiResourceEntity apiResource);

        public Task<bool> AddUser(User user, string defaultPassword);
        public Task<bool> AddRole(Role role);

    }
}
