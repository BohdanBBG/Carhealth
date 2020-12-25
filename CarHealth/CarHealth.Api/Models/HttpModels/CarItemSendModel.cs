using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Api.Models.HttpModels
{
    public class CarItemSendModel
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public int DetailMileage { get; set; }

        [Required]
        public int ChangeRide { get; set; }

        [Required]
        public int PriceOfDetail { get; set; }

        [Required]
        public DateTime Replaced { get; set; }

        [Required]
        public DateTime ReplaceAt { get; set; }

        [Required]
        public int RecomendedReplace { get; set; }
    }
}
