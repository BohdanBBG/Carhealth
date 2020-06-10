using IdentityServer4.EntityFramework.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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
            IdentityResourceId = ObjectId.GenerateNewId().ToString();
            IdentityResource = new IdentityResource();
        }
        public IdentityResourceEntity(IdentityResource resource)
        {
            IdentityResourceId = ObjectId.GenerateNewId().ToString();
            IdentityResource = resource;
        }

        public IdentityResource IdentityResource { get; set; }

        [Key]
        [BsonId]
        [Required]
        public string IdentityResourceId { get; set; }
    }
}
