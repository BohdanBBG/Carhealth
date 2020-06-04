using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.SeedServices
{
    public interface ISeedService
    {
        Task SeedAsync();
        Task RemoveDatabaseAsync();
    }
}
