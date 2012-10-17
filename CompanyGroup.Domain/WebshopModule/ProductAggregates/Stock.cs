using System;

namespace CompanyGroup.Domain.WebshopModule
{
    public class Stock
    {
        public Stock() : this(0, 0, 0) { }

        public Stock(int inner, int outer, int serbian) 
        {
            this.Inner = inner;
            this.Outer = outer;
            this.Serbian = serbian;
        }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Inner", Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public int Inner { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Outer", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public int Outer { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Serbian", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public int Serbian { get; set; }
    }
}
