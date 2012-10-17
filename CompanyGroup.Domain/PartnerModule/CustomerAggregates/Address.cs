using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// ősosztály a címadatokhoz
    /// </summary>
    public class Address
    {
        public Address(string city, string country, string zipCode, string street)
        {
            this.City = city;
            this.Country = country;
            this.ZipCode = zipCode;
            this.Street = street;
        }

        public Address() : this("", "", "", "") { }

        /// <summary>
        /// város
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("City")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string City { get; set; }

        /// <summary>
        /// országkód
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Country")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Country { get; set; }

        /// <summary>
        /// irányítószám
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("ZipCode")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string ZipCode { get; set; }

        /// <summary>
        /// utca, házszám
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Street")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Street { get; set; }

    }
}
