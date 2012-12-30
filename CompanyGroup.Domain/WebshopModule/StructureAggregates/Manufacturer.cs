using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// gyártó 
    /// </summary>
    public class Manufacturer : CompanyGroup.Domain.Core.ValueObject<Manufacturer>
    {
        public Manufacturer(string manufacturerId, string manufacturerName, string manufacturerEnglishName)
        {
            this.ManufacturerId = manufacturerId;

            this.ManufacturerName = manufacturerName;

            this.ManufacturerEnglishName = manufacturerEnglishName;
        }
        
        public Manufacturer() : this(String.Empty, String.Empty, String.Empty) { }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("ManufacturerId", Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string ManufacturerId { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("ManufacturerName", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string ManufacturerName { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("ManufacturerEnglishName", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string ManufacturerEnglishName { get; set; }
    }
}
