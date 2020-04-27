using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Models
{
    public class NewCarModel
    {
        public string CarEntityName { get; set; }
        public string CarsTotalRide { get; set; }

        public bool IsCurrent { get; set; }
    }
}
