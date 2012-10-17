using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    public class DeliveryAddress : CompanyGroup.Domain.Core.EntityBase
    {

        public DeliveryAddress(long recId, string city, string street, string zipCode, string countryRegionId)
        {
            this.RecId = recId;

            this.City = city;

            this.Street = street;

            this.ZipCode = zipCode;

            this.CountryRegionId = countryRegionId;
        }

        public DeliveryAddress() : this(0, "", "", "", "") { }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("RecId", Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public long RecId { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("City", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string City { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Street", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Street { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("ZipCode", Order = 4)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string ZipCode { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("CountryRegionId", Order = 5)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string CountryRegionId { get; set; }

        /// <summary>
        /// entitás tranziens vizsgálat
        /// </summary>
        /// <returns>Igaz ha az entitás tranziens, egyébként hamis</returns>
        public override bool IsTransient()
        {
            return RecId == 0;
        }

        /// <summary>
        /// override-olt egyenlőség vizsgálat
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is DeliveryAddress))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            DeliveryAddress item = (DeliveryAddress)obj;

            if (item.IsTransient() || this.IsTransient())
            {
                return false;
            }
            else
            {
                return item.RecId == this.RecId;
            }
        }

        /// <summary>
        /// hash code
        /// </summary>
        /// <returns></returns>
        public override int GetRequestedHashCode()
        {
            return this.RecId.GetHashCode() ^ 31;
        }
    }
}
