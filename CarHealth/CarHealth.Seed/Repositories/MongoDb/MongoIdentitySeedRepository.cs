using CarHealth.Seed.Contexts;
using CarHealth.Seed.Models.IdentityModels;
using CarHealth.Seed.Models.IdentityServer4Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Repositories.MongoDb
{
    public class MongoIdentitySeedRepository : IIdentitySeedRepository
    {

        private IMongoDatabase _database { get; set; }
        private IMongoClient _client { get; set; }

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;


        public MongoIdentitySeedRepository(
            IMongoClient client,
            string database,

            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _client = client;
            _database = client.GetDatabase(database);

            _userManager = userManager;
            _roleManager = roleManager;
        }

        private IMongoCollection<ApiResourceEntity> _apiResourceCollection
        {
            get { return _database.GetCollection<ApiResourceEntity>("ApiResourceEntities"); }
        }
        private IMongoCollection<IdentityResourceEntity> _identityResourceCollection
        {
            get { return _database.GetCollection<IdentityResourceEntity>("IdentityResourceEntities"); }
        }
        private IMongoCollection<ClientEntity> _clientCollection
        {
            get { return _database.GetCollection<ClientEntity>("ClientEntities"); }
        }
        private IMongoCollection<Role> _roleCollection
        {
            get { return _database.GetCollection<Role>("Roles"); }
        }
        private IMongoCollection<User> _usersCollection
        {
            get { return _database.GetCollection<User>("Users"); }
        }

        public bool IsApiResourceCollectionEmpty()
        {
            var builder = new FilterDefinitionBuilder<ApiResourceEntity>();
            var filter = builder.Empty; // фильтр для выборки всех документов

            return !_apiResourceCollection.Find(filter).Any();
        }

        public bool IsClientCollectionEmpty()
        {
            var builder = new FilterDefinitionBuilder<ClientEntity>();
            var filter = builder.Empty; // фильтр для выборки всех документов

            return !_clientCollection.Find(filter).Any();
        }

        public bool IsIdentityResourceCollectionEmpty()
        {
            var builder = new FilterDefinitionBuilder<IdentityResourceEntity>();
            var filter = builder.Empty; // фильтр для выборки всех документов

            return !_identityResourceCollection.Find(filter).Any();
        }

        public bool IsRoleExist(string name)
        {
            var builder = new FilterDefinitionBuilder<Role>();
            var filter = builder.Eq("NormalizedName", name.ToUpper()); 

            return _roleCollection.Find(filter).Any();
        }

        public bool IsUserExist(string email)
        {
            var builder = new FilterDefinitionBuilder<User>();
            var filter = builder.Eq("NormalizedEmail", email.ToUpper());

            return _usersCollection.Find(filter).Any();
        }


        public async Task<bool> AddApiResourceAsync(ApiResourceEntity apiResource)
        {
            await _apiResourceCollection.InsertOneAsync(apiResource);

            return true;
        }

        public async Task<bool> AddClientAsync(ClientEntity client)
        {
            await _clientCollection.InsertOneAsync(client);

            return true;
        }

        public async Task<bool> AddIdentityResourceAsync(IdentityResourceEntity identityResource)
        {
            await _identityResourceCollection.InsertOneAsync(identityResource);

            return true;
        }

        public async Task<bool> AddRole(Role role)
        {

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                var errorList = result.Errors.ToList();
                throw new Exception(string.Join("; ", errorList));
            }

            return true;

        }

        public async Task<bool> AddUser(User user, string defaultPassword)
        {
            var result = await _userManager.CreateAsync(user, defaultPassword);

            if (result.Succeeded && user.Email == "admin1@gmail.com")
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            if (!result.Succeeded)
            {
                var errorList = result.Errors.ToList();
                throw new Exception(string.Join("; ", errorList));
            }
            else
            {
                return true;
            }

        }
      
    }
}
