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

namespace CarHealth.Seed.SeedServices
{
    public class SeedServiceDevelopmentLocalhost: ISeedService
    {

        private readonly ApplicationSettings _config;
        private readonly ILogger<ISeedService> _logger;
        private readonly IDbFileReader<List<CarEntity>> _carTxtImporter;
        private readonly ICarRepository _carRepository;
        private readonly IIdentityServerConfig _identityServerConfig;
        private readonly IdentityServerContext _identityContex;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedServiceDevelopmentLocalhost(
              IOptions<ApplicationSettings> config,
              ILogger<ISeedService> logger,
              IDbFileReader<List<CarEntity>> carTxtImporter,
              ICarRepository carRepository,
              IIdentityServerConfig identityServerConfig,
              IdentityServerContext identityContex,
              UserManager<User> userManager,
              RoleManager<IdentityRole> roleManager
          )
        {
            _config = config.Value;
            _logger = logger;
            _carTxtImporter = carTxtImporter;
            _carRepository = carRepository;
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
            if (!_identityContex.Clients.Any())
            {
                foreach (var client in _identityServerConfig.GetClients(_config))
                {

                    ClientEntity clientEntity = new ClientEntity();
                    clientEntity.Client = client;
                    clientEntity.AddDataToEntity();

                    await _identityContex.Clients.AddAsync(clientEntity);
                    await _identityContex.SaveChangesAsync();
                }
            }
            _logger.LogInformation("Clients Done.");

            // IdentityResource
            _logger.LogInformation("IdentityResources...");
            if (!_identityContex.IdentityResources.Any())
            {
                foreach (var resource in _identityServerConfig.GetIdentityResources())
                {

                    IdentityResourceEntity identityResourceEntity = new IdentityResourceEntity();
                    identityResourceEntity.IdentityResource = resource;
                    identityResourceEntity.AddDataToEntity();

                    await _identityContex.IdentityResources.AddAsync(identityResourceEntity);
                    await _identityContex.SaveChangesAsync();
                }
            }
            _logger.LogInformation("IdentityResources Done.");

            // ApiResource
            _logger.LogInformation("ApiResources...");
            if (!_identityContex.ApiResources.Any())
            {
                foreach (var api in _identityServerConfig.GetApiResources())
                {

                    ApiResourceEntity apiResourceEntity = new ApiResourceEntity();
                    apiResourceEntity.ApiResource = api;
                    apiResourceEntity.AddDataToEntity();

                    await _identityContex.AddAsync(apiResourceEntity);
                    await _identityContex.SaveChangesAsync();
                }
            }
            _logger.LogInformation("ApiResources Done.");

            //Roles
            _logger.LogInformation("Roles...");

            foreach (var role in _identityServerConfig.GetInitialIdentityRoles())
            {
                var existing = _roleManager.FindByNameAsync(role.Name).GetAwaiter().GetResult();
                if (existing == null)
                {
                    _logger.LogInformation($"Role '{role.Name}': creating.");
                    var result = _roleManager.CreateAsync(role);
                    if (!result.Result.Succeeded)
                    {
                        var errorList = result.Result.Errors.ToList();
                        throw new Exception(string.Join("; ", errorList));
                    }
                }
                else
                {
                    _logger.LogInformation($"Role '{role.Name}': exists.");
                }
            }

            _logger.LogInformation("Roles Done.");

            //Users
            _logger.LogInformation("Users...");


            foreach (var user in _identityServerConfig.GetInitialdentityUsers())
            {
                if (await _userManager.FindByNameAsync(user.Email) == null)
                {
                    _logger.LogInformation($"User '{user.Email}': creating.");
                    var result = await _userManager.CreateAsync(user, _identityServerConfig.DefaultUserPassword);

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
                }
                else
                {
                    _logger.LogInformation($"User '{user.Email}': exists.");
                }
            }
            _logger.LogInformation("Users Done.");

        }

        private async Task SeedMainDb()
        {

            if(!_carRepository.IsEmptyDb())
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

                        await _carRepository.AddUserNewCarAsync(carEntity);

                        foreach (var details in car.CarItems)
                        {

                            await _carRepository.AddNewCarItemAsync(new CarItem
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
