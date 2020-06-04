using Carhealth.Seed;
using CarHealth.Seed.Models;
using CarHealth.Seed.Models.IdentityModels;
using CarHealth.Seed.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Seed
{
    public class SeedServiceDevelopmentLocalhost: ISeedService
    {

        private readonly ApplicationSettings _config;
       // private readonly Lexiconner.IdentityServer4.ApplicationSettings _identityConfig;
       // private readonly ILogger<ISeedService> _logger;
        private readonly IRepository<List<CarEntity>> _carTxtImporter;
        private readonly ICarRepository _carRepository;
       // private readonly IIdentityDataRepository _identityRepository;
       // private readonly IIdentityServerConfig _identityServerConfig;

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedServiceDevelopmentLocalhost(
              IOptions<ApplicationSettings> config,
             // IOptions<Lexiconner.IdentityServer4.ApplicationSettings> identityConfig,
             // ILogger<ISeedService> logger,
              IRepository<List<CarEntity>> carTxtImporter,
              ICarRepository carRepository,
             // IIdentityDataRepository identityRepository,
             // IIdentityServerConfig identityServerConfig,
              UserManager<User> userManager,
              RoleManager<IdentityRole> roleManager
          )
        {
            _config = config.Value;
           // _identityConfig = identityConfig.Value;
          //  _logger = logger;
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
            await SeedIdentityDb();
            await SeedMainDb();
        }

        private async Task SeedIdentityDb()

        {
            string adminEmail = "admin1@gmail.com";
            string user1Email = "user1@gmail.com";

            string password = "1234";

            if (await _roleManager.FindByNameAsync("Admin") == null)
            {
                await _roleManager.CreateAsync(new Role("Admin"));
            }

            if (await _userManager.FindByNameAsync("User") == null)
            {
                await _roleManager.CreateAsync(new Role("User"));
            }

            if (await _userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };

                var result = await _userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");

                }
            }

            if (await _userManager.FindByNameAsync(user1Email) == null)
            {
                User user1 = new User { Email = user1Email, UserName = user1Email };

                var result = await _userManager.CreateAsync(user1, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user1, "User");
                }
            }
        }

        private async Task SeedMainDb()
        {
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
        }
    }
}
