using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// gyártó 
    /// </summary>
    public class Manufacturer : CompanyGroup.Domain.Core.ValueObject<Manufacturer>
    {
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
