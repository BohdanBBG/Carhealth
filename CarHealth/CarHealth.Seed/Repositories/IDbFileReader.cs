using CarHealth.Seed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Repositories
{
    public interface IDbFileReader<T>
    {
        T ImportAllData();

    }
 }
