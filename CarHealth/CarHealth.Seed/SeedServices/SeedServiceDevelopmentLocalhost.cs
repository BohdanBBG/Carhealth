using Carhealth.Seed;
using CarHealth.Seed.Contexts;
using CarHealth.Seed.Models;
using CarHealth.Seed.Models.IdentityServer4Models;
using CarHealth.Seed.Repositories;
using IdentityServer4.EntityFramework;
using CarHealth.Seed.SeedServices.IdentityServer;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarHealth.Seed.Models.IdentityModels;

namespace CarHealth.Seed.SeedServices
{
    public class SeedServiceDevelopmentLocalhost: ISeedService
    {

        private readonly ApplicationSettings _config;
        private readonly ILogger<ISeedService> _logger;
        private readonly IDbFileReader<List<CarEntity>> _carTxtImporter;
        private readonly ISeedRepository _seedRepository;
        private readonly IIdentityServerConfig _identityServerConfig;

        private readonly IIdentitySeedRepository _identityContex;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public SeedServiceDevelopmentLocalhost(
              IOptions<ApplicationSettings> config,
              ILogger<ISeedService> logger,
              IDbFileReader<List<CarEntity>> carTxtImporter,
              ISeedRepository carRepository,
              IIdentityServerConfig identityServerConfig,

              IIdentitySeedRepository identityContex,
              UserManager<User> userManager,
              RoleManager<Role> roleManager
          )
        {
            _config = config.Value;
            _logger = logger;
            _carTxtImporter = carTxtImporter;
            _seedRepository = carRepository;
            _identityServerConfig = identityServerConfig;
            _identityContex = identityContex;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task RemoveDatabaseAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SeedAsync()
        {
            _logger.LogInformation("Start seeding data...");

            await SeedIdentityDb();
            await SeedMainDb();

            _logger.LogInformation("End seeding data...");

        }


        private async Task SeedIdentityDb()
        {
            _logger.LogInformation("\n\n");
            _logger.LogInformation("Start seeding identity DB...");


            // Client
            _logger.LogInformation("Clients...");
            if (!_identityContex.IsClientCollectionEmpty())
            {
                foreach (var client in _identityServerConfig.GetClients(_config))
                {

                    ClientEntity clientEntity = new ClientEntity();
                    clientEntity.Client = client;
                    clientEntity.AddDataToEntity();

                    await _identityContex.AddClientAsync(clientEntity);
                   
                }
            }
            _logger.LogInformation("Clients Done.");

            // IdentityResource
            _logger.LogInformation("IdentityResources...");
            if (!_identityContex.IsIdentityResourceCollectionEmpty())
            {
                foreach (var resource in _identityServerConfig.GetIdentityResources())
                {

                    IdentityResourceEntity identityResourceEntity = new IdentityResourceEntity();
                    identityResourceEntity.IdentityResource = resource;
                    identityResourceEntity.AddDataToEntity();

                    await _identityContex.AddIdentityResourceAsync(identityResourceEntity);
                }
            }
            _logger.LogInformation("IdentityResources Done.");

            // ApiResource
            _logger.LogInformation("ApiResources...");
            if (!_identityContex.IsApiResourceCollectionEmpty())
            {
                foreach (var api in _identityServerConfig.GetApiResources())
                {

                    ApiResourceEntity apiResourceEntity = new ApiResourceEntity();
                    apiResourceEntity.ApiResource = api;
                    apiResourceEntity.AddDataToEntity();

                    await _identityContex.AddApiResourceAsync(apiResourceEntity);

                }
            }
            _logger.LogInformation("ApiResources Done.");

            //Roles
            _logger.LogInformation("Roles...");

            foreach (var role in _identityServerConfig.GetInitialIdentityRoles())
            {
                if (_identityContex.IsRoleExist(role.Name))
                {
                    _logger.LogInformation($"Role '{role.Name}': exists.");
                }
                else
                {
                    _logger.LogInformation($"Role '{role.Name}': creating.");

                    _identityContex.AddRole(role).Wait();
                }

            }

            _logger.LogInformation("Roles Done.");

            //Users
            _logger.LogInformation("Users...");


            foreach (var user in _identityServerConfig.GetInitialdentityUsers())
            {
                if ( _identityContex.IsUserExist(user.Email))
                {
                    _logger.LogInformation($"User '{user.Email}': exists.");
                }
                else
                {
                    _identityContex.AddUser(user, _identityServerConfig.DefaultUserPassword).Wait();

                    _logger.LogInformation($"User '{user.Email}': creating.");
                }
            }
            _logger.LogInformation("Users Done.");

        }

        private async Task SeedMainDb()
        {

            if(!_seedRepository.IsEmptyDb())
            {
                _logger.LogInformation("\n\n");
                _logger.LogInformation("Main DB is already exists...");

                return;
            }

            _logger.LogInformation("\n\n");
            _logger.LogInformation("Start seeding main DB...");

            var carEntities = _carTxtImporter.ImportAllData();
           

            var users = _userManager.Users.ToList();

            foreach (var car in carEntities)
            {
                foreach (var user in users)
                {

                    for (int i = 0; i < 2; i++)
                    {
                        CarEntity carEntity;
                        if (i == 1)
                        {
                            carEntity = new CarEntity
                            {
                                Id = ObjectId.GenerateNewId().ToString(),
                                CarEntityName = "Car2",
                                IsCurrent = true,
                                CarsTotalRide = car.CarsTotalRide,
                                UserId = user.Id.ToString(),
                            };
                        }
                        else
                        {
                            carEntity = new CarEntity
                            {
                                Id = ObjectId.GenerateNewId().ToString(),
                                CarEntityName = car.CarEntityName,
                                IsCurrent = true,
                                CarsTotalRide = car.CarsTotalRide,
                                UserId = user.Id.ToString(),
                            };
                        }

                        await _seedRepository.AddUserNewCarAsync(carEntity);

                        foreach (var details in car.CarItems)
                        {

                            await _seedRepository.AddNewCarItemAsync(new CarItem
                            {
                                CarEntityId = carEntity.Id,
                                CarItemId = ObjectId.GenerateNewId().ToString(),
                                Name = details.Name,
                                TotalRide = details.TotalRide,
                                ChangeRide = details.ChangeRide,
                                PriceOfDetail = details.PriceOfDetail,
                                DateOfReplace = details.DateOfReplace,
                                RecomendedReplace = details.RecomendedReplace,
                                CarEntity = carEntity
                            }, user.Id.ToString());

                        }
                    }
                }
            }

            _logger.LogInformation("Done.");
        }
    }
}
