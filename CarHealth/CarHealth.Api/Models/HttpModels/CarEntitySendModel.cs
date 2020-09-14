using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Api.Models.HttpModels
{
    public class CarEntitySendModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CarEntityName { get; set; }

        [Required]
        public int TotalRide { get; set; }

        [Required]
        public bool IsDefault { get; set; }// indicate that this CarEntity will be used by Controller like default CarEntity

    }
}
