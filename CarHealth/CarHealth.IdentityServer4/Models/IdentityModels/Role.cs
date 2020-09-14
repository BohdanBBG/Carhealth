using AspNetCore.Identity.Mongo.Model;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.IdentityServer4.Models.IdentityModels
{
    //Add any custom field for a role
    public class Role : MongoIdentityRole<string> // MongoDb data repository
  // public class Role : IdentityRole // EF Core data repository
    {
      
    }
}
