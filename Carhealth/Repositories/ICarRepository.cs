using Carhealth.Models;
using Carhealth.Models.HttpModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Repositories
{
    public interface ICarRepository
    {


        public Task<List<CarEntitySendModel>> GetAllUsersCarsAsync(string userId);
        public Task<bool> SetUserCurCarAsync(ChangeUserCurrentCarModel changeModel, string userId);
        public Task<CarEntity> GetCurrentCarAsync(string userId);
        public Task AddUserNewCarAsync(NewCarModel carEntity, string userId);
        public Task<bool> UpdateUserCarAsync(EditCarModel carEntity, string userId);
        public Task<bool> DeleteUserCarAsync(string carEntityId, string userId);
        public Task<CarItemsSendModel> GetCarItemsAsync (int offset, int limit, string userId);
        public Task<CarTotalRideModel> GetTotalRideAsync(string userId);
        public Task<bool> SetTotalRideAsync(UpdateTotalRideModel value, string userId);
        public Task<bool> AddNewCarItemAsync(NewCarItemModel data, string userId);
        public Task<bool> UpdateCarItemAsync(UpdateCarItemModel value, string userId);
        public Task<bool> DeleteCarItemAsync(string detailId, string userId);
    }
}
