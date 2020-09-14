using AspNetCore.Identity.Mongo.Model;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Api.Models.IdentityModels
{
    public class User : MongoIdentityUser<string>
    //public class User : IdentityUser
    {

    }
}
