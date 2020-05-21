using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Api.Models
{
    public class UpdateTotalRideModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public int TotalRide { get; set; }
    }

}
