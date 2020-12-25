using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CarHealth.Seed.Models
{
    public class CarEntity: BaseEntity
    {

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CarName { get; set; }

        [Required]
        public int Mileage { get; set; }

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
