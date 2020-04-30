using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Models.HttpModels
{
    public class CarItemsSendModel
    {
        public int CountCarsItems { get; set; }
        public string CarEntityId { get; set; }
        public IEnumerable<CarItemSendModel> CarItems { get; set; }

      
    }
}
