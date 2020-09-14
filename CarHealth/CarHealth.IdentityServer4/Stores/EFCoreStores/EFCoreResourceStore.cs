using CarHealth.IdentityServer4.Models;
using IdentityServer4.EntityFramework.Mappers;
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
        private IdentityServerContext _identityDb { get; set; }

        public EFCoreResourceStore(IdentityServerContext identityDb)
        {
            _identityDb = identityDb;
        }

        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            var apiResource = _identityDb.ApiResources.First(t => t.ApiResource.Name == name);

            return Task.FromResult(apiResource.ApiResource);
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));


            var apiResources = new List<ApiResource>();
            var apiResourcesEntities = from i in _identityDb.ApiResources
                                       where scopeNames.Contains(i.ApiResource.Name)
                                       select i;

            foreach (var apiResourceEntity in apiResourcesEntities)
            {

                apiResources.Add(apiResourceEntity.ApiResource);
            }

            return Task.FromResult(apiResources.AsEnumerable());
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));

            var identityResources = new List<IdentityResource>();
            var identityResourcesEntities = from i in _identityDb.IdentityResources
                                            where scopeNames.Contains(i.IdentityResource.Name)
                                            select i;

            foreach (var identityResourceEntity in identityResourcesEntities)
            {

                identityResources.Add(identityResourceEntity.IdentityResource);
            }

            return Task.FromResult(identityResources.AsEnumerable());
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            var apiResourcesEntities = _identityDb.ApiResources.ToList();
            var identityResourcesEntities = _identityDb.IdentityResources.ToList();

            var apiResources = new List<ApiResource>();
            var identityResources = new List<IdentityResource>();

            foreach (var apiResourceEntity in apiResourcesEntities)
            {
                apiResources.Add(apiResourceEntity.ApiResource);
            }

            foreach (var identityResourceEntity in identityResourcesEntities)
            {
                identityResources.Add(identityResourceEntity.IdentityResource);
            }

            var result = new Resources(identityResources, apiResources);
            return Task.FromResult(result);
        }
    }
}
