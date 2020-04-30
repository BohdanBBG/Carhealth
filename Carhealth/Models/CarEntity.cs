using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Models
{
    public class CarEntity
    {
        [BsonId]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string CarEntityName { get; set; }
        public int CarsTotalRide { get; set; }

        public bool IsCurrent { get; set; }// indicate that this CarEntity will be used by Controller like default CarEntity

        [BsonIgnore]
        public List<CarItem> CarItems { get; set; }


        public string UserId { get; set; }

        public CarEntity()
        {
            this.CarItems = new List<CarItem>();
        }

    }
}
