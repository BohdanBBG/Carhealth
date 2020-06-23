using CarHealth.Seed.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using CarHealth.Seed.Contexts;

namespace CarHealth.Seed.Repositories.EFCoreDb
{
    public class EFMainDbSeedRepository: ISeedRepository
    {
        private CarContext _db { get; set; }

        public EFMainDbSeedRepository( CarContext db)
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
        

    }
}
