using Carhealth.Models;
using Carhealth.Models.HttpModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Repositories
{
    public class EFCarRepository: ICarRepository
    {
        private CarContext _db { get; set; }

        public EFCarRepository( CarContext db)
        {
            _db = db;
        }

        public async Task<List<CarEntitySendModel>> GetAllUsersCarsAsync(string userId)
        {
            List<CarEntitySendModel> result = await _db.CarEntities.Where(x => x.UserId == userId).Select(x => new CarEntitySendModel
            {
                CarEntityName = x.CarEntityName,
                Id = x.Id,
                TotalRide = x.CarsTotalRide,
                IsDefault = x.IsCurrent

            }).ToListAsync();

            if (result != null)
            {
                return result;
            }

            return null;
        }
        public async Task<bool> SetUserCurCarAsync(ChangeUserCurrentCarModel changeModel, string userId)
        {

            if (await _db.CarEntities.AnyAsync(x => x.Id == changeModel.CarEntityId && x.UserId == userId))
            {
                await _db.CarEntities.Where(x => x.UserId == userId).ForEachAsync(x => x.IsCurrent = false);

                _db.CarEntities.FirstOrDefault(x => x.Id == changeModel.CarEntityId).IsCurrent = true;

                await _db.SaveChangesAsync();

                return true;
            }

            return false;
        }
        public async Task<CarEntity> GetCurrentCarAsync(string userId)
        {
            if (await _db.CarEntities.AnyAsync(x => x.UserId == userId))
            {
                var car = await _db.CarEntities.Include(x => x.CarItems).FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == userId);

                return car;

            }

            return null;
        }
        public async Task AddUserNewCarAsync(NewCarModel carEntity, string userId)
        {

            if (!carEntity.IsCurrent && await _db.CarEntities.AnyAsync(x => x.UserId == userId))
            {
                await _db.CarEntities.AddAsync(new CarEntity
                {
                    CarEntityName = carEntity.CarEntityName,
                    CarsTotalRide = int.Parse(carEntity.CarsTotalRide),
                    IsCurrent = carEntity.IsCurrent,
                    UserId = userId,
                    Id = Guid.NewGuid().ToString()
                });
            }
            else
            {
                await _db.CarEntities.Where(x => x.UserId == userId).ForEachAsync(x => x.IsCurrent = false);

                await _db.CarEntities.AddAsync(new CarEntity
                {
                    CarEntityName = carEntity.CarEntityName,
                    CarsTotalRide = int.Parse(carEntity.CarsTotalRide),
                    IsCurrent = true,
                    UserId = userId,
                    Id = Guid.NewGuid().ToString()
                });
            }

                await _db.SaveChangesAsync();
        }
        public async Task<bool> UpdateUserCarAsync(EditCarModel carEntity, string userId)
        {

            var car = await _db.CarEntities.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == carEntity.Id);

            if (car != null)
            {
                if (!carEntity.IsCurrent)
                {
                    car.CarEntityName = carEntity.CarEntityName;
                    car.IsCurrent = carEntity.IsCurrent;
                }
                else
                {
                    await _db.CarEntities.Where(x => x.UserId == userId).ForEachAsync(x => x.IsCurrent = false);

                    car.CarEntityName = carEntity.CarEntityName;
                    car.IsCurrent = true;
                }

                await _db.SaveChangesAsync();

                return true;
            }

            return false;
        }
        public async Task<bool> DeleteUserCarAsync(string carEntityId, string userId)
        {
            var carToDelete = _db.CarEntities.FirstOrDefault(x => x.UserId == userId && x.Id == carEntityId);

            if (carToDelete != null)
            {
                if (carToDelete.IsCurrent)
                {
                    _db.CarEntities.FirstOrDefault(x => x.UserId == userId && x.Id != carToDelete.Id).IsCurrent = true;
                }

                _db.CarItems.RemoveRange(_db.CarItems.Where(x => x.CarEntityId == carToDelete.Id));

                _db.CarEntities.Remove(carToDelete);

                await _db.SaveChangesAsync();

                return true;
            }

            return false;
        }
        public async Task<CarItemsSendModel> GetCarItemsAsync(int offset, int limit, string userId)
        {
            var car = await _db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == userId);

            if (offset >= 0 &&
            limit > 0 &&
            offset <= await _db.CarItems.CountAsync() &&
            car != null
            )
            {
                var carEntitySendData = new CarItemsSendModel
                {
                    CountCarsItems = await _db.CarItems.Where(x => x.CarEntityId == car.Id).CountAsync(),
                    CarItems = _db.CarItems.Where(x => x.CarEntityId == car.Id).Skip(offset).Take(limit).Select(x => new CarItemSendModel
                    {
                        CarItemId = x.CarItemId,
                        Name = x.Name,
                        TotalRide = x.TotalRide,
                        ChangeRide = x.ChangeRide,
                        PriceOfDetail = x.PriceOfDetail,
                        DateOfReplace = x.DateOfReplace,
                        RecomendedReplace = x.RecomendedReplace

                    })
                };
                return carEntitySendData;
            }
            return null;
        }
        public async Task<CarTotalRideModel> GetTotalRideAsync(string userId)
        {
            var car = await _db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == userId);

            if (car != null)
            {
                return new CarTotalRideModel
                {
                    CarsTotalRide = car.CarsTotalRide
                };
            }

            return null;
        }
        public async Task<bool> SetTotalRideAsync(UpdateTotalRideModel value, string userId)
        {
            var carEntity = await _db.CarEntities.FirstOrDefaultAsync(x => x.Id == value.Id && x.UserId == userId);

            if (value.TotalRide < carEntity.CarsTotalRide)
            {
                return true;
            }

            if (value.TotalRide > 0 &&
                carEntity != null &&
                value.TotalRide > carEntity.CarsTotalRide
                )
            {
                int carEntityTotalRide = carEntity.CarsTotalRide;

                await _db.CarItems.Where(x => x.CarEntityId == carEntity.Id).
                    ForEachAsync(item =>
                    {
                        item.TotalRide += (value.TotalRide - carEntityTotalRide);
                    });

                carEntity.CarsTotalRide = value.TotalRide;

                await _db.SaveChangesAsync();

                return true;
            }

            return false;
        }
        public async Task<bool> AddNewCarItemAsync(NewCarItemModel data, string userId)
        {
            if (await _db.CarEntities.AnyAsync(x => x.UserId == userId))
            {
                await _db.CarItems.AddAsync(new CarItem
                {
                    CarItemId = Guid.NewGuid().ToString(),
                    CarEntityId = _db.CarEntities.FirstOrDefault(x => x.IsCurrent == true && x.UserId == userId).Id,
                    Name = data.Name,
                    TotalRide = 0,
                    ChangeRide = int.Parse(data.ChangeRide),
                    PriceOfDetail = int.Parse(data.PriceOfDetail),
                    RecomendedReplace = int.Parse(data.RecomendedReplace),
                    DateOfReplace = DateTime.Parse(data.DateOfReplace)

                });

                await _db.SaveChangesAsync();

                return true;
            }
            return false;
        }
        public async Task<bool> UpdateCarItemAsync(UpdateCarItemModel value, string userId)
        {
            var carEntity = await _db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == userId);

            if (carEntity != null)
            {
                var carItem = await _db.CarItems.FirstOrDefaultAsync(x => x.CarEntityId == carEntity.Id && x.CarItemId == value.CarItemId);

                if (carItem != null)
                {
                    carItem.Name = value.Name;
                    carItem.TotalRide = value.IsTotalRideChanged ? 0 : carItem.TotalRide;
                    carItem.ChangeRide = int.Parse(value.ChangeRide);
                    carItem.PriceOfDetail = int.Parse(value.PriceOfDetail);
                    carItem.DateOfReplace = DateTime.Parse(value.DateOfReplace);
                    carItem.RecomendedReplace = int.Parse(value.RecomendedReplace);

                    await _db.SaveChangesAsync();

                    return true;
                }
            }
            return false;
        }
        public async Task<bool> DeleteCarItemAsync(string detailId, string userId)
        {
            var carEntity = await _db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == userId);

            if (carEntity != null)
            {
                var carItemToDelete = await _db.CarItems.FirstOrDefaultAsync(x => x.CarEntityId == carEntity.Id && x.CarItemId == detailId);

                if (carItemToDelete != null)
                {
                    _db.CarItems.Remove(carItemToDelete);

                    await _db.SaveChangesAsync();

                    return true;
                }

            }
            return false;
        }

    }
}
