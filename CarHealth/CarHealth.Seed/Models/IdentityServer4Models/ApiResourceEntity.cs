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
    public class ApiResourceEntity
    {

        public ApiResourceEntity()
        {
            ApiResourceId = ObjectId.GenerateNewId().ToString();
            ApiResource = new ApiResource();
        }
        public ApiResourceEntity(ApiResource resource)
        {
            ApiResourceId = ObjectId.GenerateNewId().ToString();
            ApiResource = resource;
        }

        public ApiResource ApiResource { get; set; }

        [Key]
        [BsonId]
        [Required]
        public string ApiResourceId { get; set; }
    }
}
