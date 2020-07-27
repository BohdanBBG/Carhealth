using Bogus;
using CarHealth.Api.Models;
using CarHealth.Api.Models.IdentityModels;
using CarHealth.Api.Repositories;
using CarHealth.ApiTest.TestRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public CarEntity TakeCarEntity(bool current = false)
        {
            return new CarEntity
            {
                // UserId = ?
                CarEntityName = _faker.Lorem.Word(),
                CarsTotalRide = new Random().Next(),
                IsCurrent = current
            };
        }

        public async Task<CarEntity> GetCurrentCarEntity(string userId)
        {
            _dataRepository.UserId = userId;

            return await _dataRepository.GetCurrentCarAsync();
        }

        public async Task<List<CarEntity>> GetAllUsersCarsAsync(string userId)
        {
            _dataRepository.UserId = userId;

            return await _dataRepository.GetAllUsersCarsAsync();
        }
        
        public List<CarEntity> CreateCarEntityInTestRepo(string userId, int count = 1)
        {
            _dataRepository.UserId = userId;

            int countAddedCarEntities = -1;

            var carEntities = Enumerable.Range(0, count).Select(x =>
            {
                countAddedCarEntities++;

                return new CarEntity
                {
                    UserId = userId,
                    CarEntityName = _faker.Lorem.Word(),
                    CarsTotalRide = new Random().Next(),
                    IsCurrent = countAddedCarEntities == 0 ? true : false,
                };

            }).ToList();

            foreach(var c in carEntities)
            {
                _dataRepository.AddUserNewCarAsync(c);
            }

          //  carEntities.ForEach(x => _dataRepository.AddUserNewCarAsync(x));

            return carEntities;
        }

        public CarEntity PrepareStudyItemUpdateDto(CarEntity entity)
        {
            return new CarEntity
            {
                CarEntityName = _faker.Lorem.Word(),
                CarsTotalRide = new Random().Next(),
                IsCurrent = entity.IsCurrent == false ? false : true
            };
        }

        // TODO add data managment for caritems

    }
}
