﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Models
{
    public class NewCarItemModel
    {
        public string CarEntityId { get; set; }
        public string Name { get; set; }
        public string ChangeRide { get; set; }
        public string PriceOfDetail { get; set; }
        public string DateOfReplace { get; set; }
        public string RecomendedReplace { get; set; }

    }
}