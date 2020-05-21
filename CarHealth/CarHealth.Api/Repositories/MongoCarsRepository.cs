using CarHealth.Api.Models;
using CarHealth.Api.Models.HttpModels;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Api.Repositories
{
    public class MongoCarsRepository : ICarRepository
    {
        private IMongoDatabase _database { get; set; }
        private IMongoClient _client { get; set; }


        public MongoCarsRepository(IMongoClient client, string database )
        {
            _client = client;
            _database = client.GetDatabase(database);

        }

        public IMongoCollection<CarEntity> CarEntities
        {
            get { return _database.GetCollection<CarEntity>("CarEntity"); }
        }

        public IMongoCollection<CarItem> CarItems
        {
            get { return _database.GetCollection<CarItem>("CarItem"); }
        }

        public bool IsEmptyDb()
        {
            // строитель фильтров
            var builder = new FilterDefinitionBuilder<CarEntity>();
            var filter = builder.Empty; // фильтр для выборки всех документов

            return !CarEntities.Find(filter).Any();
        }

        public async Task<List<CarEntity>> GetAllUsersCarsAsync(string userId)
        {
            // return await CarEntities.Find(x => x.UserId == userId).ToListAsync();
            return await CarEntities.Find(new BsonDocument("UserId", userId)).ToListAsync();
        }

        public async Task<bool> SetUserCurCarAsync(string carEntityId, string userId)
        {
            if (carEntityId != null && userId != null)
            {
                var builder = Builders<CarEntity>.Filter;
                var filter = builder.Eq("UserId", userId) & builder.Eq("Id", carEntityId);

                await CarEntities.UpdateManyAsync(builder.Eq("UserId", userId), new BsonDocument("$set", new BsonDocument("IsCurrent", false)));

                var result = await CarEntities.UpdateOneAsync(
                       filter,
                     new BsonDocument("$set", new BsonDocument("IsCurrent", true)));

                if (result.ModifiedCount > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<CarEntity> GetCurrentCarAsync(string userId)
        {
            var builder = Builders<CarEntity>.Filter;
            var filter = builder.Eq("UserId", userId) & builder.Eq("IsCurrent", true);

            var res = await CarEntities.Find(filter).FirstOrDefaultAsync();

            if (res != null)
            {
                return res;
            }

            return null;
        }

        public async Task AddUserNewCarAsync(CarEntity carEntity)
        {
            if (carEntity.IsCurrent)
            {
                var builder = Builders<CarEntity>.Filter;

                await CarEntities.UpdateManyAsync(builder.Eq("UserId", carEntity.UserId), new BsonDocument("$set", new BsonDocument("IsCurrent", false)));
            }

            if ( !await CarEntities.Find(x => x.UserId == carEntity.UserId).AnyAsync())
            {
                carEntity.IsCurrent = true;
            }

            await this.SetTotalRideAsync(new UpdateTotalRideModel
            {
                Id = carEntity.Id,
                TotalRide = carEntity.CarsTotalRide
            }, carEntity.UserId);

            await CarEntities.InsertOneAsync(carEntity);
        }

        public async Task<bool> UpdateUserCarAsync(EditCarModel carEntity, string userId)
        {
            var builderFilter = Builders<CarEntity>.Filter;
            var builderUpdate = Builders<CarEntity>.Update;

            var filterUser = builderFilter.Eq("UserId", userId);
            var filterCarEntity = builderFilter.Eq("Id", carEntity.Id);
            var update = builderUpdate.Set("CarEntityName", carEntity.CarEntityName);

            var car = await CarEntities.Find(filterUser & filterCarEntity).FirstOrDefaultAsync();

            if (car != null)
            {
                await this.SetTotalRideAsync(new UpdateTotalRideModel
                {
                    Id = car.Id,
                    TotalRide = carEntity.CarsTotalRide
                }, userId);

                await CarEntities.UpdateOneAsync(filterUser & filterCarEntity, update);

                if (carEntity.IsCurrent &&
                    !(car.IsCurrent))
                {
                    await SetUserCurCarAsync(carEntity.Id, userId);
                }

                if (!carEntity.IsCurrent &&
                   car.IsCurrent)
                {
                    await SetUserCurCarAsync(CarEntities.Find(filterUser & !filterCarEntity).FirstOrDefault().Id, userId); 
                }
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteUserCarAsync(string carEntityId, string userId)
        {
            var car = await CarEntities.Find(x => x.Id == carEntityId && x.UserId == userId).FirstOrDefaultAsync();

            if (car != null)
            {

                if (car.IsCurrent)
                {
                    await SetUserCurCarAsync(CarEntities.Find(x => x.Id != carEntityId && x.UserId == userId).FirstOrDefault().Id, userId);
                }

                var result = await CarEntities.DeleteOneAsync(x => x.Id == carEntityId && x.UserId == userId);

                await CarItems.DeleteManyAsync(x => x.CarEntityId == carEntityId);

                return true;
            }

            return false;
        }

        public async Task<CarItemsSendModel> GetCarItemsAsync(int offset, int limit, string userId)
        {
            var car = await CarEntities.Find(x => x.UserId == userId && x.IsCurrent == true).FirstOrDefaultAsync();

            if (car != null)
            {
                var carItems = await CarItems.Find(x => x.CarEntityId == car.Id).ToListAsync();
                int carItemsCount = carItems.Count();
                if (
                    offset >= 0 &&
                    limit > 0 
                   )
                {
                    var carEntitySendData = new CarItemsSendModel
                    {
                        CountCarsItems = carItemsCount,
                        CarEntityId = car.Id,
                        CarItems = carItemsCount == 0 || carItemsCount <= offset ? null :
                        carItems.Skip(offset).Take(limit).Select(x => new CarItemSendModel
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
            }
            return null;
        }

        public async Task<CarTotalRideModel> GetTotalRideAsync(string userId)
        {
            var car = await CarEntities.Find(x => x.UserId == userId && x.IsCurrent == true).FirstOrDefaultAsync();

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
            var carEntity = await CarEntities.Find(x => x.UserId == userId && x.Id == value.Id).FirstOrDefaultAsync();

            if (carEntity != null)
            {
                if (value.TotalRide <= carEntity.CarsTotalRide)
                {
                    return true;
                }

                if (value.TotalRide > 0)
                {
                    int carEntityTotalRide = carEntity.CarsTotalRide;

                    var filterCarEntity = Builders<CarEntity>.Filter.Eq("Id", carEntity.Id);
                    var filterCarItem = Builders<CarItem>.Filter.Eq("CarEntityId", carEntity.Id);

                    var updateCarItemTotalRide = Builders<CarItem>.Update.Inc<int>("TotalRide", (value.TotalRide - carEntityTotalRide));
                    var updateCarEntityTotalRide = Builders<CarEntity>.Update.Set("CarsTotalRide", value.TotalRide);


                    var resultCarItem = await CarItems.UpdateManyAsync(filterCarItem, updateCarItemTotalRide);
                    var resultCarEntity = await CarEntities.UpdateOneAsync(filterCarEntity, updateCarEntityTotalRide);

                    if (resultCarEntity.ModifiedCount > 0 && resultCarItem.ModifiedCount > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<bool> AddNewCarItemAsync(CarItem data, string userId)
        {
            var car = await CarEntities.Find(x => x.UserId == userId && x.Id == data.CarEntityId).FirstOrDefaultAsync();

            if (car != null)
            {
                //await this.SetTotalRideAsync(new UpdateTotalRideModel
                //{
                //    Id = data.CarEntityId,
                //    TotalRide = car.CarsTotalRide
                //}, userId);

                await CarItems.InsertOneAsync(data);

                return true;
            }

            return false;
        }

        public async Task<bool> UpdateCarItemAsync(UpdateCarItemModel value, string userId)
        {
            var carEntity = await CarEntities.Find(x => x.UserId == userId && x.IsCurrent == true).FirstOrDefaultAsync();

            if (carEntity != null)
            {
                //await this.SetTotalRideAsync(new UpdateTotalRideModel
                //{
                //    Id = carEntity.Id,
                //    TotalRide = carEntity.CarsTotalRide
                //}, userId);

                var carItem = CarItems.Find(x => x.CarItemId == value.CarItemId).FirstOrDefault();

                var filterCarItem = Builders<CarItem>.Filter.Eq("CarItemId", value.CarItemId);

                await CarItems.ReplaceOneAsync(filterCarItem, new CarItem
                {
                    CarItemId = carItem.CarItemId,
                    Name = value.Name,
                    TotalRide = value.IsTotalRideChanged ? 0 : carItem.TotalRide,
                    ChangeRide = int.Parse(value.ChangeRide),
                    PriceOfDetail = int.Parse(value.PriceOfDetail),
                    DateOfReplace = DateTime.Parse(value.DateOfReplace),
                    RecomendedReplace = int.Parse(value.RecomendedReplace),
                    CarEntityId = carItem.CarEntityId
                });

                return true;
            }
            return false;
        }

        public async Task<bool> DeleteCarItemAsync(string detailId, string userId)
        {
            var carEntity = await CarEntities.Find(x => x.IsCurrent == true && x.UserId == userId).FirstOrDefaultAsync();

            if (carEntity != null)
            {
                var result = await CarItems.DeleteOneAsync(x => x.CarItemId == detailId && x.CarEntityId == carEntity.Id);

                if (result.DeletedCount > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
