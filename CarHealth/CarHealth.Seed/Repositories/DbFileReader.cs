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
    public class DbFileReader: IDbFileReader<List<CarEntity>>
    {
        private string _filePath;

        public DbFileReader(string filePath)
        {
            _filePath = filePath;
        }


        public List<CarEntity> ImportAllData()
        {
            List<CarEntity> carEntities = new List<CarEntity>
            {
                JsonSerializer.Deserialize<CarEntity>(File.ReadAllText(_filePath))
            };

            return carEntities;
        }


    }
}
