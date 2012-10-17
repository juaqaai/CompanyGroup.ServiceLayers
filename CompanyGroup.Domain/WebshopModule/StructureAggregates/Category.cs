using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// termék kategória
    /// </summary>
    public class Category : CompanyGroup.Domain.Core.ValueObject<Category>
    {
        [MongoDB.Bson.Serialization.Attributes.BsonElement("CategoryId", Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string CategoryId { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("CategoryName", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string CategoryName { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("CategoryEnglishName", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string CategoryEnglishName { get; set; }
    }
}
