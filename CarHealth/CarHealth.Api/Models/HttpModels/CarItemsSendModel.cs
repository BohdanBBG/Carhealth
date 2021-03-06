﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Api.Models.HttpModels
{
    public class CarItemsSendModel
    {
        [Required]
        public int CountCarsItems { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CarEntityId { get; set; }

        [Required]
        public List<CarItemSendModel> CarItems { get; set; }

      
    }
}
