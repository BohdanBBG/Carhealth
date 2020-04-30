using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Carhealth.Models
{
    public class CarItem
    {

        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [BsonId]
        public string CarItemId { get; set; }
        public string Name { get; set; }
        public int TotalRide { get; set; }
        public int ChangeRide { get; set; }
        public int PriceOfDetail { get; set; }
        public DateTime DateOfReplace { get; set; }
        public int RecomendedReplace { get; set; }

        
        public string CarEntityId { get; set; }
        [BsonIgnore]
        [NotMapped]
        public CarEntity CarEntity { get; set; }

    }

   
}
