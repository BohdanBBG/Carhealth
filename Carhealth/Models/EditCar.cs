using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Models
{
    public class EditCar
    {
        public string Id { get; set; }
        public string CarEntityName { get; set; }
        public bool IsCurrent { get; set; }// indicate that this CarEntity will be used by Controller like default CarEntity

    }
}
