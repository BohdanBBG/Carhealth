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
    public class ClientEntity
    {
        public ClientEntity()
        {
            ClientId = ObjectId.GenerateNewId().ToString();
        }
        public ClientEntity(Client client)
        {
            ClientId = ObjectId.GenerateNewId().ToString();
            this.Client = client;
        }

        public Client Client { get; set; }

        [Key]
        [BsonId]
        [Required]
        public string ClientId { get; set; }
    }
}
