using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Models
{
    public class UpdateCarItemModel
    {
        [Required]
        public string CarItemId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public bool IsTotalRideChanged { get; set; }

        [Required]
        public string ChangeRide { get; set; }

        [Required]
        public string PriceOfDetail { get; set; }

        [Required]
        public string DateOfReplace { get; set; }

        [Required]
        public string RecomendedReplace { get; set; }
    }
}
