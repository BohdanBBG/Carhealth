using CarHealth.IdentityServer4.Models;
using CarHealth.IdentityServer4.Models.IdentityServer4Models;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.IdentityServer4.Stores.MongoDbStores
{
    public class MongoDbClientStore : IClientStore
    {
        private IMongoDatabase _database { get; set; }
        private IMongoClient _mongoClient { get; set; }

        public MongoDbClientStore(IMongoClient mongoClient, string database)
        {
            _mongoClient = mongoClient;
            _database = _mongoClient.GetDatabase(database);
        }

        private IMongoCollection<ClientEntity> _clientCollection
        {
            get { return _database.GetCollection<ClientEntity>("ClientEntities"); }
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {

            var client = await _clientCollection.Find(x => x.Client.ClientId == clientId).FirstOrDefaultAsync();

            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.Client;

        }

    }
}
