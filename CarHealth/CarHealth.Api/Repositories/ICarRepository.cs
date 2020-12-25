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
        public bool IsEmptyDb();

        public Task<List<CarEntity>> GetAllUsersCarsAsync(string userId);
        public Task<bool> SetUserCurCarAsync(string carEntityId, string userId);
        public Task<CarEntity> GetCurrentCarAsync(string userId);
        public Task AddUserNewCarAsync(CarEntity carEntity);
        public Task<bool> UpdateUserCarAsync(EditCarModel carEntity, string userId);
        public Task<bool> DeleteUserCarAsync(string carEntityId, string userId);
        public Task<IList<CarItem>> FindCarItem(string name, string userId);
        public Task<CarItemsSendModel> GetCarItemsAsync(int offset, int limit, string userId);
        public Task<CarTotalRideModel> GetTotalRideAsync(string userId);
        public Task<bool> SetTotalRideAsync(UpdateCarMiliageModel value, string userId);
        public Task<bool> AddNewCarItemAsync(CarItem data, string userId);
        public Task<bool> UpdateCarItemAsync(UpdateCarItemModel value, string userId);
        public Task<bool> DeleteCarItemAsync(string detailId, string userId);
    }
}