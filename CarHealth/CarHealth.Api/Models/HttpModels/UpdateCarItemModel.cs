using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Api.Models
{
    public class UpdateCarItemModel
    {
        [Required]
        public string Id { get; set; }

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
        public string Replaced { get; set; }

        [Required]
        public string ReplaceAt { get; set; }

        [Required]
        public string RecomendedReplace { get; set; }
    }
}
