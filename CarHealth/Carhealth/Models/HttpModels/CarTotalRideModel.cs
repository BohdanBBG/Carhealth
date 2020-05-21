using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Models
{
    public class CarTotalRideModel
    {
        [Required]
        public int CarsTotalRide { get; set; }
    }
}
