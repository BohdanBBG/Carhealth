using System;
using System.Collections.Generic;
using System.Linq;
using CarHealth.Seed.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CarHealth.Seed.Models
{
    public class CarContext: DbContext
    {

        public CarContext(DbContextOptions<CarContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<CarEntity> CarEntities { get; set; }
        public DbSet<CarItem> CarItems { get; set; }

    }

}
