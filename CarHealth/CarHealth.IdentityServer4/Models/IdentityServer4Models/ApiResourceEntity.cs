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

namespace CarHealth.IdentityServer4.Models.IdentityServer4Models
{
    public class ApiResourceEntity
    {

        public ApiResourceEntity()
        {
            Id = Guid.NewGuid().ToString();
        }

        public ApiResourceEntity(ApiResource resource)
        {
            Id = Guid.NewGuid().ToString();
            ApiResource = resource;
        }

        public string Id { get; set; }

        [NotMapped]
        public ApiResource ApiResource { get; set; }

    }
}
