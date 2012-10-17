using System;

namespace CompanyGroup.Domain.WebshopModule
{
    public class Garanty
    {
        public Garanty() : this("", "") { }

        public Garanty(string time, string mode)
        {
            this.Time = time;
            this.Mode = mode;
        }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Time", Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Time { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Mode", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Mode { get; set; }
    }
}
