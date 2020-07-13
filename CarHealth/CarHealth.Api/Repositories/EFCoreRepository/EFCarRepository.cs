﻿using CarHealth.Api.Models;
using CarHealth.Api.Models.HttpModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using CarHealth.Api.Contexts;

namespace CarHealth.Api.Repositories.EFCoreRepository
{
    public class EFCarRepository : ICarRepository
    {
        private CarContext _db { get; set; }
        public string UserId { get; set; }

        public EFCarRepository(CarContext db)
        {
            _db = db;
        }

        public bool IsEmptyDb()
        {
            return !_db.CarEntities.Any();
        }

        public async Task<List<CarEntity>> GetAllUsersCarsAsync()
        {
            List<CarEntity> result = await _db.CarEntities.Where(x => x.UserId == UserId).ToListAsync();

            if (result != null)
            {
                return result;
            }

            return null;
        }
        public async Task<bool> SetUserCurCarAsync(string carEntityId )
        {

            if (await _db.CarEntities.AnyAsync(x => x.Id == carEntityId && x.UserId == UserId))
            {
                await _db.CarEntities.Where(x => x.UserId == UserId).ForEachAsync(x => x.IsCurrent = false);

                _db.CarEntities.FirstOrDefault(x => x.Id == carEntityId).IsCurrent = true;

                await _db.SaveChangesAsync();

                return true;
            }

            return false;
        }
        public async Task<CarEntity> GetCurrentCarAsync()
        {
            if (await _db.CarEntities.AnyAsync(x => x.UserId == UserId))
            {
                var car = await _db.CarEntities.Include(x => x.CarItems).FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == UserId);

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
        public async Task<bool> UpdateUserCarAsync(EditCarModel carEntity)
        {

            var car = await _db.CarEntities.FirstOrDefaultAsync(x => x.UserId == UserId && x.Id == carEntity.Id);

            if (car != null)
            {
                if (carEntity.IsCurrent)
                {
                    car.CarEntityName = carEntity.CarEntityName;
                    await this.SetUserCurCarAsync(carEntity.Id);
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
        public async Task<bool> DeleteUserCarAsync(string carEntityId)
        {
            var carToDelete = _db.CarEntities.FirstOrDefault(x => x.UserId == UserId && x.Id == carEntityId);

            if (carToDelete != null)
            {
                if (carToDelete.IsCurrent)
                {
                    var car = _db.CarEntities.FirstOrDefault(x => x.UserId == UserId && x.Id != carToDelete.Id);

                    if (car != null)
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

        public async Task<IList<CarItem>> FindCarItem(string name)
        {
            var car = await _db.CarEntities.FirstOrDefaultAsync(x => x.UserId == UserId && x.IsCurrent == true);

            if (car != null)
            {
                Regex regex = new Regex(name);

                return await _db.CarItems.Where(x => EF.Functions.Like(x.Name, "%" + name + "%") && x.CarEntityId == car.Id).ToListAsync();
            }
            return null;
        }

        public async Task<CarItemsSendModel> GetCarItemsAsync(int offset, int limit)
        {
            var car = await _db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == UserId);

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
        public async Task<CarTotalRideModel> GetTotalRideAsync()
        {
            var car = await _db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == UserId);

            if (car != null)
            {
                return new CarTotalRideModel
                {
                    CarsTotalRide = car.CarsTotalRide
                };
            }

            return null;
        }
        public async Task<bool> SetTotalRideAsync(UpdateTotalRideModel value)
        {

            var carEntity = await _db.CarEntities.FirstOrDefaultAsync(x => x.Id == value.Id && x.UserId == UserId);

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
        public async Task<bool> AddNewCarItemAsync(CarItem data)
        {
            if (await _db.CarEntities.AnyAsync(x => x.UserId == UserId && x.Id == data.CarEntityId))
            {
                await _db.CarItems.AddAsync(data);

                await _db.SaveChangesAsync();

                return true;
            }
            return false;
        }
        public async Task<bool> UpdateCarItemAsync(UpdateCarItemModel value)
        {
            var carEntity = await _db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == UserId);

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
        public async Task<bool> DeleteCarItemAsync(string detailId )
        {
            var carEntity = await _db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == UserId);

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