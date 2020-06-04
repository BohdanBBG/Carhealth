using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarHealth.Seed.Models;
using System.IO;
using System.Text.Json;

namespace CarHealth.Seed.Repositories
{
    public class FileRepository: IRepository<List<CarEntity>>
    {
        //private string _filePath = "./AppData/data.json";
        private string _filePath;

        public FileRepository(string filePath)
        {
            _filePath = filePath;
            //_hostingEnvironment = hostingEnvironment;
            // Path.Combine(_hostingEnvironment.ContentRootPath, _filePath);
        }


        public List<CarEntity> ImportAllData()
        {
            List<CarEntity> carEntities = new List<CarEntity>
            {
                JsonSerializer.Deserialize<CarEntity>(File.ReadAllText(_filePath))
            };

            return carEntities;
        }

        public bool RecalcCarItemsRides(string idCarEntity, int totalRideDiff)
        {
            List<CarEntity> carEntities = new List<CarEntity>
            {
                JsonSerializer.Deserialize<CarEntity>(File.ReadAllText(_filePath))
            };

            if (carEntities != null)
            {

               carEntities.Find(x => x.Id == idCarEntity).CarItems.ForEach(item =>
               {
                  item.TotalRide += totalRideDiff;
               });

               this.UpdateAllData(carEntities);

               return true;
            }

            return false;
        }

        public void UpdateAllData(List<CarEntity> carEntities)
        {
            File.WriteAllText(_filePath, JsonSerializer.Serialize<List<CarEntity>>( carEntities));
             
        }

    }
}
