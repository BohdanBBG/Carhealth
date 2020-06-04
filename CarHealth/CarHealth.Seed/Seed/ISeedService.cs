using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Seed
{
    interface ISeedService
    {
        Task SeedAsync();
        Task RemoveDatabaseAsync();
    }
}
