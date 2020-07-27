using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarHealth.Seed.Models
{
    public class CarEntity: BaseEntity
    {
      

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CarEntityName { get; set; }

        [Required]
        public int CarsTotalRide { get; set; }

        [Required]
        public bool IsCurrent { get; set; }// indicate that this CarEntity will be used by Controller like default CarEntity

        [BsonIgnore]
        public List<CarItem> CarItems { get; set; }

        [Required]
        public string UserId { get; set; }

        public CarEntity()
        {
            this.CarItems = new List<CarItem>();
        }

    }
}
