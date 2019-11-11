using Carhealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.ImportAndExport
{
    public interface IRepository
    {
        List<CarEntity> carEntities { get; set; }

        CarEntity Import();

        void UpdateAllFileData(CarEntity carEntity);

        void ReCalcCarItemsRides(CarEntity carEntity, int carsTotalRide);

    }
}
