using System;

namespace CompanyGroup.Domain.WebshopModule
{
    public class Stock
    {
        public Stock() : this(0, 0) { }

        public Stock(int inner, int outer) 
        {
            this.Inner = inner;
            this.Outer = outer;
        }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Inner", Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public int Inner { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Outer", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public int Outer { get; set; }
    }
}
