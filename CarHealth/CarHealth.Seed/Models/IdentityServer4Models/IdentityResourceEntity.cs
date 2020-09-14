using IdentityServer4.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Models.IdentityServer4Models
{
    public class IdentityResourceEntity
    {
        public IdentityResourceEntity()
        {
            Id = Guid.NewGuid().ToString(); 
        }

        public IdentityResourceEntity(IdentityResource resource)
        {
            Id = Guid.NewGuid().ToString();
            IdentityResource = resource;
        }

        public string Id { get; set; }

        [NotMapped]
        public IdentityResource IdentityResource { get; set; }

    }
}
