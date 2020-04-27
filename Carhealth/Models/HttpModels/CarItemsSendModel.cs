using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Models.HttpModels
{
    public class CarItemsSendModel
    {
        public int CountCarsItems { get; set; }
        public IQueryable<CarItemSendModel> CarItems { get; set; }

      
    }
}
