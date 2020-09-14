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
    public class ClientEntity
    {

        public ClientEntity()
        {
            Id = Guid.NewGuid().ToString();
        }

        public ClientEntity(Client client)
        {
            Id = Guid.NewGuid().ToString();
            this.Client = client;
        }

        public string Id { get; set; }

        [NotMapped]
        public Client Client { get; set; }


    }
}
