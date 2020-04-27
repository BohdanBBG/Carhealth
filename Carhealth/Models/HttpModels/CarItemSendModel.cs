using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Models.HttpModels
{
    public class CarItemSendModel
    {
        public string CarItemId { get; set; }
        public string Name { get; set; }
        public int TotalRide { get; set; }
        public int ChangeRide { get; set; }
        public int PriceOfDetail { get; set; }
        public DateTime DateOfReplace { get; set; }
        public int RecomendedReplace { get; set; }
    }
}
