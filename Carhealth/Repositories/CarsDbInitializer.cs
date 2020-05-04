using Carhealth.Models;
using Carhealth.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Repositories
{
    public class CarsDbInitializer
    {
        public static async Task InitializeAsync(IRepository<List<CarEntity>> fileRepository, UserManager<User> userManager, ICarRepository repository)
        {
            var carEntities = fileRepository.ImportAllData();

            var users = userManager.Users.ToList();

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

                        await repository.AddUserNewCarAsync(carEntity);

                        foreach (var details in car.CarItems)
                        {

                            await repository.AddNewCarItemAsync(new CarItem
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
