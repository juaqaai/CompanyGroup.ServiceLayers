using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// kiszállítási adatok a rendeléshez
    /// </summary>
    public class Shipping
    {
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("DateRequested", Order = 1)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(MongoDB.Bson.BsonDateTime.Create(DateTime.MinValue))]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public DateTime DateRequested { get; set; }

        //[MongoDB.Bson.Serialization.Attributes.BsonElement("ZipCode", Order = 2)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string ZipCode { get; set; }

        //[MongoDB.Bson.Serialization.Attributes.BsonElement("City", Order = 3)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string City { get; set; }

        //[MongoDB.Bson.Serialization.Attributes.BsonElement("Country", Order = 4)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Country { get; set; }

        //[MongoDB.Bson.Serialization.Attributes.BsonElement("Street", Order = 5)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Street { get; set; }

        //[MongoDB.Bson.Serialization.Attributes.BsonElement("AddrRecId", Order = 6)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public long AddrRecId { get; set; }

        //[MongoDB.Bson.Serialization.Attributes.BsonElement("InvoiceAttached", Order = 7)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool InvoiceAttached { get; set; }


    }
}
