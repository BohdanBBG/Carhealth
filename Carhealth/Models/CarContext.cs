using System;
using System.Collections.Generic;
using Carhealth.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Carhealth.Models
{
    public class CarContext: DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<List<CarEntity>> _repository;

        public CarContext(IConfiguration configuration, IRepository<List<CarEntity>> repository)
        {
            this._configuration = configuration;
            this._repository = repository;

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

            foreach (var car in carEntities)
            {
                modelBuilder.Entity<CarEntity>().HasData(new {car.Id, car.CarEntityName, car.CarsTotalRide });

                foreach (var details in car.CarItems)
                {
                    modelBuilder.Entity<CarItem>().HasData(details);
                }
            }
        }
    }

}
