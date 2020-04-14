using Carhealth.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Repositories
{
    public class CarsDbInitializer
    {
        public static async Task InitializeAsync(IRepository<List<CarEntity>> repository, UserManager<User> userManager, CarContext carContext)
        {
            var carEntities = repository.ImportAllData();

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
                                Id = Guid.NewGuid().ToString(),
                                CarEntityName = "Car2",
                                IsCurrent = false,
                                CarsTotalRide = car.CarsTotalRide,
                                UserId = user.Id,
                            };
                        }
                        else
                        {
                            carEntity = new CarEntity
                            {
                                Id = Guid.NewGuid().ToString(),
                                CarEntityName = car.CarEntityName,
                                IsCurrent = true,
                                CarsTotalRide = car.CarsTotalRide,
                                UserId = user.Id,
                            };
                        }

                        carContext.CarEntities.Add(carEntity);

                        foreach (var details in car.CarItems)
                        {
                            carContext.CarItems.Add( new CarItem
                            {
                                CarEntityId = carEntity.Id,
                                CarItemId = Guid.NewGuid().ToString(),
                                Name = details.Name,
                                TotalRide = details.TotalRide,
                                ChangeRide = details.ChangeRide,
                                PriceOfDetail = details.PriceOfDetail,
                                DateOfReplace = details.DateOfReplace,
                                RecomendedReplace = details.RecomendedReplace,
                                CarEntity = carEntity
                            });
                        }
                        await carContext.SaveChangesAsync();

                    }
                }
            }
        }
    }
}
