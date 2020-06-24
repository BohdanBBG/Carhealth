using CarHealth.Seed.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Repositories
{
    public class MongoMainDbSeedRepository : ISeedRepository
    {
        private IMongoDatabase _database { get; set; }
        private IMongoClient _client { get; set; }


        public MongoMainDbSeedRepository(IMongoClient client, string database )
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
          
            var builder = new FilterDefinitionBuilder<CarEntity>();
            var filter = builder.Empty; // фильтр для выборки всех документов

            return !CarEntities.Find(filter).Any();
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


            await CarEntities.InsertOneAsync(carEntity);
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
      
    }
}
