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

        public bool IsEmptyDb()
        {
            return  !_db.CarEntities.Any();
        }

        public async Task<List<CarEntity>> GetAllUsersCarsAsync(string userId)
        {
            List<CarEntity> result = await _db.CarEntities.Where(x => x.UserId == userId).ToListAsync();

            if (result != null)
            {
                return result;
            }
          
            return null;
        }
        public async Task<bool> SetUserCurCarAsync(string carEntityId, string userId)
        {

            if (await _db.CarEntities.AnyAsync(x => x.Id == carEntityId && x.UserId == userId))
            {
                await _db.CarEntities.Where(x => x.UserId == userId).ForEachAsync(x => x.IsCurrent = false);

                _db.CarEntities.FirstOrDefault(x => x.Id == carEntityId).IsCurrent = true;

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
        public async Task AddUserNewCarAsync(CarEntity carEntity)
        {
            if (!carEntity.IsCurrent)
            {
                await _db.CarEntities.AddAsync(carEntity);
            }
            else
            {
                await _db.CarEntities.Where(x => x.UserId == carEntity.UserId).ForEachAsync(x => x.IsCurrent = false);

                await _db.CarEntities.AddAsync(carEntity);
            }

            await _db.SaveChangesAsync();

        }
        public async Task<bool> UpdateUserCarAsync(EditCarModel carEntity, string userId)
        {

            var car = await _db.CarEntities.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == carEntity.Id);

            if (car != null)
            {
                if(carEntity.IsCurrent)
                {
                    car.CarEntityName = carEntity.CarEntityName;
                    await this.SetUserCurCarAsync(carEntity.Id, userId);
                }
                else
                {
                    car.CarEntityName = carEntity.CarEntityName;
                    car.IsCurrent = false;
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
                    var car = _db.CarEntities.FirstOrDefault(x => x.UserId == userId && x.Id != carToDelete.Id);

                    if(car != null)
                    {
                        car.IsCurrent = true;
                    }
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
                    CarEntityId = car.Id,
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

            if (carEntity != null)
            {
                if (value.TotalRide < carEntity.CarsTotalRide)
                {
                    return true;
                }

                if (value.TotalRide > 0 &&
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
            }
            return false;
        }
        public async Task<bool> AddNewCarItemAsync(CarItem data, string userId)
        {
            if (await _db.CarEntities.AnyAsync(x => x.UserId == userId && x.Id == data.CarEntityId))
            {
                await _db.CarItems.AddAsync(data);

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
