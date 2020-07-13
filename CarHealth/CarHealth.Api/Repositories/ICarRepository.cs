using CarHealth.Api.Models;
using CarHealth.Api.Models.HttpModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Api.Repositories
{
    public interface ICarRepository
    {
        public string UserId { get; set; }

        public bool IsEmptyDb();

        public Task<List<CarEntity>> GetAllUsersCarsAsync();
        public Task<bool> SetUserCurCarAsync(string carEntityId);
        public Task<CarEntity> GetCurrentCarAsync();
        public Task AddUserNewCarAsync(CarEntity carEntity);
        public Task<bool> UpdateUserCarAsync(EditCarModel carEntity);
        public Task<bool> DeleteUserCarAsync(string carEntityId);
        public Task<IList<CarItem>> FindCarItem(string name);
        public Task<CarItemsSendModel> GetCarItemsAsync(int offset, int limit);
        public Task<CarTotalRideModel> GetTotalRideAsync();
        public Task<bool> SetTotalRideAsync(UpdateTotalRideModel value);
        public Task<bool> AddNewCarItemAsync(CarItem data);
        public Task<bool> UpdateCarItemAsync(UpdateCarItemModel value);
        public Task<bool> DeleteCarItemAsync(string detailId);
    }
}
