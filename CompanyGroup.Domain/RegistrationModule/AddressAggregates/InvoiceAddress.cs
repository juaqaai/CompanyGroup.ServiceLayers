using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.RegistrationModule
{
    /// <summary>
    /// számlázási cím
    /// </summary>
    public class InvoiceAddress : CompanyGroup.Domain.PartnerModule.Address
    {
        public InvoiceAddress(string city, string countryRegionId, string zipCode, string street, string phone) : base(city, countryRegionId, zipCode, street)
        {
            this.Phone = phone;
        }

        public InvoiceAddress() : this("", "", "", "", "") { }

        /// <summary>
        /// telefon
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Phone")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Phone { get; set; }
    }
}
