using CarHealth.IdentityServer4.Models;
using CarHealth.IdentityServer4.Models.IdentityServer4Models;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.IdentityServer4.Stores.MongoDbStores
{
    public class MongoDbResourceStore : IResourceStore
    {
        private IMongoDatabase _database { get; set; }
        private IMongoClient _mongoClient { get; set; }

        public MongoDbResourceStore(IMongoClient mongoClient, string database)
        {
            _mongoClient = mongoClient;
            _database = _mongoClient.GetDatabase(database);
        }

        private IMongoCollection<ApiResourceEntity> _apiResourceCollection
        {
            get { return _database.GetCollection<ApiResourceEntity>("ApiResourceEntities"); }
        }
        private IMongoCollection<IdentityResourceEntity> _identityResourceCollection
        {
            get { return _database.GetCollection<IdentityResourceEntity>("IdentityResourceEntities"); }
        }


        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var apiResource =  _apiResourceCollection.Find(x => x.ApiResource.Name == name).FirstOrDefault();

            return Task.FromResult(apiResource.ApiResource);

           
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));

            var result = _apiResourceCollection.Find(a => a.ApiResource.Scopes.Any(s => scopeNames.Contains(s.Name))).ToList();

            var apiResources = new List<ApiResource>();

            foreach (var apiResourceEntity in result)
            {

                apiResources.Add(apiResourceEntity.ApiResource);
            }
            
            return Task.FromResult(apiResources.AsEnumerable());

        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));

            var result = _identityResourceCollection.Find(e => scopeNames.Contains(e.IdentityResource.Name)).ToList();

            var identityResources = new List<IdentityResource>();

            foreach (var identityResourceEntity in result)
            {

                identityResources.Add(identityResourceEntity.IdentityResource);
            }

            return Task.FromResult(identityResources.AsEnumerable());

        }

        public Task<Resources> GetAllResourcesAsync()
        {

            var builderForApiResource = new FilterDefinitionBuilder<ApiResourceEntity>();
            var builderForIdentityResource = new FilterDefinitionBuilder<IdentityResourceEntity>();

            var filterApiResource = builderForApiResource.Empty;
            var filterIdentityResource = builderForIdentityResource.Empty; // фильтр для выборки всех документов

            var apiResourcesEntities = _apiResourceCollection.Find(filterApiResource).ToList();
            var identityResourcesEntities = _identityResourceCollection.Find(filterIdentityResource).ToList();


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
