using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Models
{
    public class UpdateCarItem
    {
        public string CarItemId { get; set; }
        public string Name { get; set; }
        public bool IsTotalRideChanged { get; set; }
        public string ChangeRide { get; set; }
        public string PriceOfDetail { get; set; }
        public string DateOfReplace { get; set; }
        public string RecomendedReplace { get; set; }
    }
}
