using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace  CarHealth.Api.Models.IdentityModels
{
    //public class Role : MongoRole // MongoDb data repository
    public class Role : IdentityRole // EF Core data repository
    {
      
    }
}
