using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carhealth.Models;
using System.IO;
using System.Text.Json;

namespace Carhealth.Repositories
{
    public class FileRepository: IRepository<List<CarEntity>>
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private string _filePath = "./AppData/data.json";


        public FileRepository(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            Path.Combine(_hostingEnvironment.ContentRootPath, _filePath);
        }


        public List<CarEntity> ImportAllData()
        {
            List<CarEntity> carEntities = new List<CarEntity>
            {
                JsonSerializer.Deserialize<CarEntity>(File.ReadAllText(_filePath))
            };

            return carEntities;
        }

        public bool RecalcCarItemsRides(int idCarEntity, int totalRideDiff)
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
