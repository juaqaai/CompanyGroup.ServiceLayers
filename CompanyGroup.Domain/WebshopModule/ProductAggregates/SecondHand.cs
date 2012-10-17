using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// domain secondhand value object
    /// </summary>
    public class SecondHand
    {
        public SecondHand() : this( "", "", 0, 0, "" ) { }

        public SecondHand(string configId, string inventLocationId, int quantity, int price, string statusDescription)
        {
            this.ConfigId = configId;

            this.InventLocationId = inventLocationId;

            this.Quantity = quantity;

            this.Price = price;

            this.StatusDescription = statusDescription;
        }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("ConfigId", Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string ConfigId { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("InventLocationId", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string InventLocationId { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Quantity", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public int Quantity { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Price", Order = 4)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public int Price { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("StatusDescription", Order = 5)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string StatusDescription { get; set; }
    }

    /// <summary>
    /// domain secondhand value object list
    /// </summary>
    public class SecondHandList : List<SecondHand> { }
}
