using Carhealth.Seed;
using CarHealth.Seed.Models;
using CarHealth.Seed.Repositories;
using Microsoft.AspNetCore.Identity;
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
       // private readonly Lexiconner.IdentityServer4.ApplicationSettings _identityConfig;
        private readonly ILogger<ISeedService> _logger;
        private readonly IMainDbSeed<List<CarEntity>> _carTxtImporter;
        private readonly ICarRepository _carRepository;
       // private readonly IIdentityDataRepository _identityRepository;
       // private readonly IIdentityServerConfig _identityServerConfig;

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedServiceDevelopmentLocalhost(
              IOptions<ApplicationSettings> config,
              // IOptions<Lexiconner.IdentityServer4.ApplicationSettings> identityConfig,
              ILogger<ISeedService> logger,
              IMainDbSeed<List<CarEntity>> carTxtImporter,
              ICarRepository carRepository,
              // IIdentityDataRepository identityRepository,
              // IIdentityServerConfig identityServerConfig,
              UserManager<User> userManager,
              RoleManager<IdentityRole> roleManager
          )
        {
            _config = config.Value;
            // _identityConfig = identityConfig.Value;
            _logger = logger;
            _carTxtImporter = carTxtImporter;
            _carRepository = carRepository;
            // _identityRepository = identityRepository;
            //_identityServerConfig = identityServerConfig;
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

            _logger.LogInformation("Start seeding data...");

        }

        private async Task SeedIdentityDb()
        {
            _logger.LogInformation("\n\n");
            _logger.LogInformation("Start seeding identity DB...");


            string password = "1234";

            var roles =  new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin"
                },
                 new IdentityRole
                {
                    Name = "User"
                }
            };

            _logger.LogInformation("Roles...");

            foreach (var role in roles)
            {
                if (await _roleManager.FindByNameAsync(role.Name) == null)
                {
                    _logger.LogInformation($"Role '{role}': creating.");
                    var result = await _roleManager.CreateAsync(role);

                    if(!result.Succeeded)
                    {
                        var errorList = result.Errors.ToList();
                        throw new Exception(string.Join("; ", errorList));
                    }
                }
                else
                {
                    _logger.LogInformation($"Role '{role.Name}': exists.");
                }
            }

            _logger.LogInformation("Roles Done.");


            _logger.LogInformation("Users...");

            var users = new List<User>
            {
                new User
                {
                    Email = "admin1@gmail.com",
                    UserName = "admin1@gmail.com"
                },
                new User
                {
                     Email = "user1@gmail.com",
                     UserName ="user1@gmail.com"
                }
            };

            _logger.LogInformation("Roles...");

            foreach (var user in users)
            {
                if (await _userManager.FindByNameAsync(user.Email) == null)
                {
                    _logger.LogInformation($"User '{user.Email}': creating.");
                    var result = await _userManager.CreateAsync(user, password);

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
