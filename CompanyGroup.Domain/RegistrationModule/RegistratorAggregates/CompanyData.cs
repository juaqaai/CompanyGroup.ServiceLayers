using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.RegistrationModule
{
    public class CompanyData : CompanyGroup.Domain.Core.ValueObject<CompanyData>
    {
        public CompanyData(string countryRegionId, string customerId, string customerName, string euVatNumber, string mainEmail, bool newsletterToMainEmail, string registrationNumber, string signatureEntityFile, string vatNumber)
        {
            this.CountryRegionId = countryRegionId;
            this.CustomerId = customerId;
            this.CustomerName = customerName;
            this.RegistrationNumber = registrationNumber;
            this.VatNumber = vatNumber;
            this.EUVatNumber = euVatNumber;
            this.SignatureEntityFile = signatureEntityFile;
            this.MainEmail = mainEmail;
            this.NewsletterToMainEmail = newsletterToMainEmail;
        }

        /// <summary>
        /// ország megnevezés
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("CountryRegionId")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string CountryRegionId { get; set; }

        /// <summary>
        /// cégazonosító
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("CustomerId")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string CustomerId { get; set; }

        /// <summary>
        /// cégnév
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("CustomerName")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string CustomerName { get; set; }

        /// <summary>
        /// cégjegyzékszám
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("RegistrationNumber")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// adószám
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("VatNumber")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string VatNumber { get; set; }

        /// <summary>
        /// uniós adószám
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("EUVatNumber")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string EUVatNumber { get; set; }

        /// <summary>
        /// aláírási címpéldány
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("SignatureEntityFile")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string SignatureEntityFile { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("MainEmail")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string MainEmail { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("NewsletterToMainEmail")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(false)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool NewsletterToMainEmail { get; set; }
    }
}
