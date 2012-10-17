using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{

    /// <summary>
    /// domain product picture
    /// </summary>
    public class Picture
    {
        [MongoDB.Bson.Serialization.Attributes.BsonElement("FileName", Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string FileName { get; set; }

        //[MongoDB.Bson.Serialization.Attributes.BsonElement("RawContent", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public byte[] RawContent { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Primary", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(false)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool Primary { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("RecId", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public long RecId { get; set; }
    }

    /// <summary>
    /// domain product pictures
    /// </summary>
    public class Pictures : List<Picture> { }
}
