using CarHealth.Seed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Repositories
{
    public interface IRepository<T>
    {
        T ImportAllData();

        void UpdateAllData(T carEntities);

        bool RecalcCarItemsRides(string idCarEntity, int totalRideDiff);

    }
 }
