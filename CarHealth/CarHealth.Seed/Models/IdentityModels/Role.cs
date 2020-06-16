using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Models.IdentityModels
{
    //Add any custom field for a role
   // public class Role : MongoRole // MongoDb data repository
   public class Role : IdentityRole // EF Core data repository
    {
       
    }
}
