using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.ViewModels
{
    public class ChangeCarBindViewModelcs
    {
        public string UserId { get; set; }
        public List<string> CarEntityId { get; set; }

        public ChangeCarBindViewModelcs()
        {
            CarEntityId = new List<string>();
        }
    }
}
