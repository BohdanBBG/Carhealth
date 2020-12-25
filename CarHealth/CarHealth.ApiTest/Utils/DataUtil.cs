using Bogus;
using CarHealth.Api.Models;
using CarHealth.Api.Models.IdentityModels;
using CarHealth.Api.Repositories;
using CarHealth.ApiTest.TestRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.ApiTest.Utils
{
    public class DataUtil<TStartup> where TStartup : class
    {
        private readonly ICarRepository _dataRepository;
        private readonly IIdentityMongoRepository<User> _identityRepository;
        private readonly Faker _faker;

        public DataUtil(CustomWebApplicationBuilder<TStartup> factory)
        {
            _identityRepository = factory.Server.Host.Services.GetRequiredService<IIdentityMongoRepository<User>>();
            _dataRepository = factory.Server.Host.Services.GetRequiredService<ICarRepository>();
            _faker = new Faker();
        }

        public async Task<User> CreateUserAsync(List<string> roles = null)
        {
            var userEntity = new User
            {
                // Id = _faker.UniqueIndex.ToString(),
                UserName = _faker.Internet.UserName(),
                Email = _faker.Internet.Email(),
                EmailConfirmed = true,
                PhoneNumber = _faker.Phone.PhoneNumber(),
                PhoneNumberConfirmed = true,
                Roles = roles ?? new List<string>()
            };

            await _identityRepository.AddAsync(userEntity);

            return userEntity;
        }


        // mainDb

        #region CarEntity

        public CarEntity CreateTestCarEntity(string userId, bool isCurrent = false)
        {
            return new CarEntity
            {
                UserId = userId,
                CarName = _faker.Lorem.Word(),
                Mileage = new Random().Next(),
                IsCurrent = isCurrent,
            };
        }

        public async Task<CarEntity> GetCurrentCarEntity(string userId)
        {

            return await _dataRepository.GetCurrentCarAsync(userId);
        }

        public async Task<List<CarEntity>> GetAllUsersCarsAsync(string userId)
        {
            return await _dataRepository.GetAllUsersCarsAsync(userId);
        }

        /// <summary>
        /// Creates and returns a list of carEntities in a test repository. The first car always has IsCurrent = true.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<List<CarEntity>> CreateCarEntityInTestRepo(string userId, int count = 1)
        {
            int countAddedCarEntities = 0;

            var carEntities = Enumerable.Range(0, count).Select(x =>
            {
                countAddedCarEntities++;

                return new CarEntity
                {
                    UserId = userId,
                    CarName = _faker.Lorem.Word(),
                    Mileage = new Random().Next(),
                    IsCurrent = countAddedCarEntities == 1,
                };

            }).ToList();


            foreach (var c in carEntities)
            {
                await _dataRepository.AddUserNewCarAsync(c);
            }

            return carEntities;
        }


        #endregion

        #region CarItem

        public List<CarItem> CreateTestCarItem(string userId, int count = 1)
        {

            var carItems = Enumerable.Range(0, count).Select(x =>
            {
                return new CarItem
                {
                    Name = _faker.Lorem.Word(),
                    DetailMileage = new Random().Next(),
                    ChangeRide = new Random().Next(),
                    PriceOfDetail = new Random().Next(),
                    Replaced = DateTime.Now,
                    ReplaceAt = DateTime.Now.AddYears(2),
                    RecomendedReplace = new Random().Next(),
                    CarEntityId = "",
                };
            }).ToList();


            return carItems;
        }

        /// <summary>
        /// Creates and returns a list of carItems in a test repository.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<List<CarItem>> CreateCarItemInTestRepo(string userId, int count = 1, List<string> names = null)
        {
            var carEntity = await this.CreateCarEntityInTestRepo(userId, 2);

            int counter = -1;

            var carItems = Enumerable.Range(0, count).Select(x =>
             {
                 counter++;

                 return new CarItem
                 {
                     Name = names != null && counter < names.Count() ?
                                             names[counter] : _faker.Lorem.Word(),
                     DetailMileage = new Random().Next(),
                     ChangeRide = new Random().Next(),
                     PriceOfDetail = new Random().Next(),
                     Replaced = DateTime.Now,
                     ReplaceAt = DateTime.Now.AddYears(2),
                     RecomendedReplace = new Random().Next(),
                     CarEntityId = carEntity.FirstOrDefault().Id,
                 };
             }).ToList();

            foreach (var item in carItems)
            {
                await _dataRepository.AddNewCarItemAsync(item, userId);
            }

            return carItems;
        }

        #endregion

    }
}
