using Carhealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Repositories
{
    public interface IRepository<T>
    {
        T ImportAllData();

        void UpdateAllData(List<CarEntity> carEntities);

        bool RecalcCarItemsRides(string idCarEntity, int totalRideDiff);

    }
}
