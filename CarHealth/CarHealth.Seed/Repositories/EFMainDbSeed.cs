using CarHealth.Seed.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Repositories
{
    public class EFMainDbSeed: ICarRepository
    {
        private CarContext _db { get; set; }

        public EFMainDbSeed( CarContext db)
        {
            _db = db;
        }

        public bool IsEmptyDb()
        {
            return  !_db.CarEntities.Any();
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
