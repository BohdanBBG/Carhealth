using CarHealth.Api.Models.IdentityModels;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbGenericRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHealth.ApiTest.TestRepositories
{
    public class IdentityMongoRepository<TUser> : IIdentityMongoRepository<TUser> where TUser: User
    {
        public IMongoClient _client { get; set; }

        public IMongoDatabase _database { get; set; }

        public IdentityMongoRepository(IMongoClient client, string database)
        {
            _client = client;
            _database = _client.GetDatabase(database);
        }


        public IMongoCollection<TUser> UserCollections
        {
            get { return _database.GetCollection<TUser>("User"); }
        }

        public async Task AddAsync(TUser user)
        {
            await UserCollections.InsertOneAsync(user);
        }



    }
}
