using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Models.HttpModels
{
    public class CarItemSendModel
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CarItemId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public int TotalRide { get; set; }

        [Required]
        public int ChangeRide { get; set; }

        [Required]
        public int PriceOfDetail { get; set; }

        [Required]
        public DateTime DateOfReplace { get; set; }

        [Required]
        public int RecomendedReplace { get; set; }
    }
}
