using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Models
{
    public class User: IdentityUser
    {
        public List<CarEntity> CarEntity { get; set; }

        public User()
        {
            CarEntity = new List<CarEntity>();
        }
    }
}
