using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Models
{
    public class EditCarModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CarEntityName { get; set; }

        [Required]
        public bool IsCurrent { get; set; }// indicate that this CarEntity will be used by Controller like default CarEntity

        [Required]
        public int CarsTotalRide { get; set; }

    }
}
