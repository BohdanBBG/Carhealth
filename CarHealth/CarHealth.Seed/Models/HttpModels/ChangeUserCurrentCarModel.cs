using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Models
{
    public class ChangeUserCurrentCarModel
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CarEntityId { get; set; }
    }
}
