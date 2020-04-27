using Carhealth.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Repositories
{
    public class MongoCarsRepository
    {
        private IMongoDatabase _database { get; set; }

        public MongoCarsRepository(IConfiguration configuration)
        {
            // строка подключения
            string connectionString = configuration.GetConnectionString("MongoDb");
            var connection = new MongoUrlBuilder(connectionString);
            // получаем клиента для взаимодействия с базой данных
            MongoClient client = new MongoClient(connectionString);
            // получаем доступ к самой базе данных
            _database = client.GetDatabase(connection.DatabaseName);

        }

        public async Task<CarEntity> GetAllUsersCarsAsync()
        {
            return null;
        }
    }
}
