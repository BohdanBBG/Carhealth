using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carhealth.Models;
using System.IO;
using System.Text.Json;

namespace Carhealth.ImportAndExport
{
    public class Repository: IRepository
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private string _filePath = "./AppData/data.json";
        public List<CarEntity> carEntities { get; set; }



        public Repository(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            Path.Combine(_hostingEnvironment.ContentRootPath, _filePath);
            carEntities = new List<CarEntity>();
        }


        public CarEntity Import()
        {
            CarEntity carEntityData = JsonSerializer.Deserialize<CarEntity>(File.ReadAllText(_filePath));
            carEntities.Add(carEntityData);
            carEntityData.CountCarsItems = carEntityData.CarDetails.Count();
           
            return carEntityData;
        }

        public void ReCalcCarItemsRides(CarEntity carEntity, int carsTotalRide)
        {
            carEntity.CarDetails.ForEach(item =>
            {
                item.TotalRide += (carsTotalRide - carEntity.CarsTotalRide);
            });
        }

        public void UpdateAllFileData(CarEntity carEntity)
        {
            File.WriteAllText(_filePath, JsonSerializer.Serialize<CarEntity>( carEntity));
             
        }

    }
}
