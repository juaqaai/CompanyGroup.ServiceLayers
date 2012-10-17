using System;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// domain product flags
    /// </summary>
    public class Flags
    {
        /// <summary>
        /// új flag
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("New", Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(false)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool New { get; set; }

        /// <summary>
        /// hírlevélben szerepel (akció)
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("IsInNewsletter", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(false)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool IsInNewsletter { get; set; }

        /// <summary>
        /// készleten van-e a cikk?
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("InStock", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(false)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool InStock { get; set; }

        /// <summary>
        /// akciós
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Action", Order = 4)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(false)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool Action { get; set; }
    }
}
