using System;
using System.Collections.Generic;
using System.Linq;
using Carhealth.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Carhealth.Models
{
    public class CarContext: DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<List<CarEntity>> _repository;
        private UserManager<User> _userManager;

        public CarContext(IConfiguration configuration, IRepository<List<CarEntity>> repository, UserManager<User> userManager)
        {
            this._configuration = configuration;
            this._repository = repository;
            _userManager = userManager;

            Database.EnsureCreated();
        }

        public DbSet<CarEntity> CarEntities { get; set; }
        public DbSet<CarItem> CarItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("CarsDb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var carEntities = _repository.ImportAllData();

            var users =  _userManager.Users.ToList();

            foreach (var car in carEntities)
            {
                foreach (var user in users)
                {
                    CarEntity carEntity = new CarEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        CarEntityName = car.CarEntityName,
                        CarsTotalRide = car.CarsTotalRide,
                        UserId = user.Id,
                    };

                    modelBuilder.Entity<CarEntity>().HasData(carEntity);

                    foreach (var details in car.CarItems)
                    {
                        modelBuilder.Entity<CarItem>().HasData(new CarItem
                        {
                            CarEntityId = carEntity.Id,
                            CarItemId = Guid.NewGuid().ToString(),
                            Name = details.Name,
                            TotalRide = details.TotalRide,
                            ChangeRide = details.ChangeRide,
                            PriceOfDetail = details.PriceOfDetail,
                            DateOfReplace = details.DateOfReplace,
                            RecomendedReplace = details.RecomendedReplace,
                            CarEntity = carEntity
                        });
                    }
                }
            }
        }
    }

}
