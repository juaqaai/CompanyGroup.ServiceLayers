using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// jogosultság beállítások
    ///     web adminisztrátor-e?
    ///     számla info engedélyezett-e?
    ///     árlista letöltés engedélyezett-e?
    ///     rendelés engedélyezett-e?
    ///     áruátvétel engedélyezett-e?
    /// </summary>
    public class Permission : CompanyGroup.Domain.Core.ValueObject<Permission>
    {
        [MongoDB.Bson.Serialization.Attributes.BsonElement("IsWebAdministrator", Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(false)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool IsWebAdministrator { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("InvoiceInfoEnabled", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(false)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool InvoiceInfoEnabled { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("PriceListDownloadEnabled", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(false)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool PriceListDownloadEnabled { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("CanOrder", Order = 4)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(false)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool CanOrder { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("RecieveGoods", Order = 5)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(false)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool RecieveGoods { get; set; }

        public Permission(bool isWebAdministrator, bool invoiceInfoEnabled, bool priceListDownloadEnabled, bool canOrder, bool recieveGoods)
        {
            this.IsWebAdministrator = isWebAdministrator;

            this.InvoiceInfoEnabled = invoiceInfoEnabled;

            this.PriceListDownloadEnabled = priceListDownloadEnabled;

            this.CanOrder = canOrder;

            this.RecieveGoods = recieveGoods;
        }

        public Permission()
        {
            this.IsWebAdministrator = false;

            this.InvoiceInfoEnabled = false;

            this.PriceListDownloadEnabled = false;

            this.CanOrder = false;

            this.RecieveGoods = false;            
        }
    }
}
