using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Api.Models
{
    public class NewCarItemModel
    {
        [Required]
        public string CarEntityId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

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
