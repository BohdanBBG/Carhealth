using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Models
{
    public class CarEntity
    {
     
        public CarEntity()
        {
           
        }

        public CarEntity(string carEntityName,int id)
        {
            this.CarId = id;
            CarEntityName = carEntityName;
        }
        public int CarId { get; set; }
        public string CarEntityName { get; set; }
        public int CarsTotalRide { get; set; }
        public int CountCarsItems { get; set; }
        public List<CarItem> CarDetails { get; set; }
    }
}
