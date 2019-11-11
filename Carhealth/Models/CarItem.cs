using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Models
{
    public class CarItem
    {
        public CarItem()
        {

        }

        public int Detail_id { get; set; }
        public string Name { get; set; }
        public int TotalRide { get; set; }
        public int ChangeRide { get; set; }
        public int PriceOfDetail { get; set; }
        public DateTime DateOfReplace { get; set; }
        public int RecomendedReplace { get; set; }

    }

   
}
