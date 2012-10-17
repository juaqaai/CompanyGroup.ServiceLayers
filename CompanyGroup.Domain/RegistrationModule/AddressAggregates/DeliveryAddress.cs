using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.RegistrationModule
{
    /// <summary>
    /// regisztrációs szállítási cím
    /// </summary>
    public class DeliveryAddress : CompanyGroup.Domain.PartnerModule.DeliveryAddress, CompanyGroup.Domain.Core.INoSqlEntity
    {
        public DeliveryAddress() : this(0, "", "", "", "") { }

        public DeliveryAddress(long recId, string city, string zipCode, string street, string countryRegionId) : base(recId, city, street, zipCode, countryRegionId) 
        {
            this.Id = MongoDB.Bson.ObjectId.GenerateNewId();
        }

        [MongoDB.Bson.Serialization.Attributes.BsonId(Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public MongoDB.Bson.ObjectId Id { set; get; }
    }
}
