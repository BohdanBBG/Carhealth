using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Api.Models
{
    public class CarItem: BaseEntity
    {


        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public int TotalRide { get; set; }

        [Required]
        public int ChangeRide { get; set; }

        [Required]
        public int PriceOfDetail { get; set; }

        [Required]
        public DateTime DateOfReplace { get; set; }

        [Required]
        public int RecomendedReplace { get; set; }

        [Required]
        public string CarEntityId { get; set; }

        [BsonIgnore]
        [NotMapped]
        public CarEntity CarEntity { get; set; }

    }

   
}
