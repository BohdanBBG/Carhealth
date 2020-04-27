using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Models.HttpModels
{
    public class CarEntitySendModel
    {
        public string Id { get; set; }
        public string CarEntityName { get; set; }
        public int TotalRide { get; set; }
        public bool IsDefault { get; set; }// indicate that this CarEntity will be used by Controller like default CarEntity

    }
}
