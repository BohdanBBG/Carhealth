using CarHealth.Seed.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Repositories
{
    public interface ICarRepository
    {
        public bool IsEmptyDb();

        
        public Task AddUserNewCarAsync(CarEntity carEntity);
        public Task<bool> DeleteUserCarAsync(string carEntityId, string userId);
        public Task<bool> AddNewCarItemAsync(CarItem data, string userId);
        public Task<bool> DeleteCarItemAsync(string detailId, string userId);
    }
}
