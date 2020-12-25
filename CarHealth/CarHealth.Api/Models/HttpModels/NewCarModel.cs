using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Api.Models
{
    public class NewCarModel
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CarName { get; set; }

        [Required]
        public string Mileage { get; set; }

        public List<CarItem> CarItems { get; set; }

        [Required]
        public bool IsCurrent { get; set; }

        public NewCarModel()
        {
            CarItems = new List<CarItem>();
        }
    }
}
