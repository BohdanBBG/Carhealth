using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CarHealth.Api.Models
{
    public class CarItem: BaseEntity
    {

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public int DetailMileage { get; set; } //ride on this detail

        [Required]
        public int ChangeRide { get; set; } //Mileage + RecomendedReplace

        [Required]
        public int RecomendedReplace { get; set; }

        [Required]
        public int PriceOfDetail { get; set; }

        [Required]
        public DateTime Replaced { get; set; }

        public DateTime ReplaceAt { get; set; }

        [Required]
        public string CarEntityId { get; set; }

        [BsonIgnore]
        [NotMapped]
        public CarEntity CarEntity { get; set; }
    }
   
}
