using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.RegistrationModule
{
    /// <summary>
    /// vevőregisztráció aggregátum
    /// </summary>
    public class Registration : CompanyGroup.Domain.Core.NoSqlEntity 
    {
        [MongoDB.Bson.Serialization.Attributes.BsonElement("VisitorId", Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string VisitorId { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("CompanyId", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string CompanyId { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("PersonId", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string PersonId { get; set; }

        /// <summary>
        /// regisztrációs adatokat rögzítő
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("DataRecording", Order = 4)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public DataRecording DataRecording { get; set; }

        /// <summary>
        /// ország, cégazonosító, cégnév, cégjegyzékszám, adószám, uniós adószám, aláírási címpéldány
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("CompanyData", Order = 5)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public CompanyData CompanyData { get; set; }

        /// <summary>
        /// számlázási cím
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("InvoiceAddress", Order = 6)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public InvoiceAddress InvoiceAddress { get; set; }


        /// <summary>
        /// levelezési cím 
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("MailAddress", Order = 7)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public MailAddress MailAddress { get; set; }

        /// <summary>
        /// webadminisztrátor (kapcsolattartó)
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("WebAdministrator", Order = 8)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public WebAdministrator WebAdministrator { get; set; }

        ///// <summary>
        ///// munkamenet azonosító
        ///// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("SessionId", Order = 14)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        //public string SessionId { get; set; }

        /// <summary>
        /// bankszámlaszámok
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("BankAccountList", Order = 9)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public List<BankAccount> BankAccountList { get; set; }

        /// <summary>
        /// szállítási címek
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("DeliveryAddressList", Order = 10)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public List<DeliveryAddress> DeliveryAddressList { get; set; }

        /// <summary>
        /// kapcsolattartók
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("ContactPersonList", Order = 11)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public List<ContactPerson> ContactPersonList { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Status", Order = 12)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public RegistrationStatus Status {get; set;}
    }
}
