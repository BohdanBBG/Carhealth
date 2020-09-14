using CarHealth.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CarHealth.Api.Contexts
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
