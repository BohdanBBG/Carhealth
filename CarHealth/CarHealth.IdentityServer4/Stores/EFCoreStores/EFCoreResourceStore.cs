using CarHealth.IdentityServer4.Models;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.IdentityServer4.Stores.EFCoreStores
{
    public class EFCoreResourceStore : IResourceStore
    {
        private IdentityContex _identityDb { get; set; }

        public EFCoreResourceStore(IdentityContex identityDb)
        {
            _identityDb = identityDb;
        }

        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

           return await _identityDb.ApiResources.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            return await _identityDb.ApiResources.Where(x => x.Scopes.Any(s => scopeNames.Contains(s.Name))).ToListAsync();
        }

        public async  Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            return await _identityDb.IdentityResources.Where(x => scopeNames.Contains(x.Name)).ToListAsync();
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            return await GetAllResources();
        }

        public async Task<Resources> GetAllResources()
        {
          return new Resources(await GetAllIdentityResources(), await GetAllApiResources());
        }

        private async Task<IEnumerable<ApiResource>> GetAllApiResources()
        {
           return  await _identityDb.ApiResources.Where(x => true).ToListAsync();
        }

        private async Task<IEnumerable<IdentityResource>> GetAllIdentityResources()
        {
            return await _identityDb.IdentityResources.Where(x => true).ToListAsync();
        }
    }
}
