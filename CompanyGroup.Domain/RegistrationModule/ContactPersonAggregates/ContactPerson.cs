using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.RegistrationModule
{
    /// <summary>
    /// regisztrációs kapcsolattartó     
    /// </summary>
    public class ContactPerson : CompanyGroup.Domain.PartnerModule.ContactPerson, CompanyGroup.Domain.Core.INoSqlEntity
    {
        public ContactPerson() : this("", "", "", "", "", "", false, false, false, false, false, false, false, false, false, false, false, "", "", false, false, 0, 0) { }

        public ContactPerson(string contactPersonId, string lastName, string firstName, string email, string cellularPhone, string phone, 
                             bool allowOrder, bool allowReceiptOfGoods, 
                             bool smsArriveOfGoods, bool smsOrderConfirm, bool smsOfDelivery, 
                             bool emailArriveOfGoods, bool emailOfOrderConfirm, bool emailOfDelivery, 
                             bool webAdmin, bool priceListDownload, bool invoiceInfo,
                             string userName, string password, bool leftCompany, bool newsletter, long recId, long refRecId)
            : base(contactPersonId, lastName, firstName, email, cellularPhone, phone, "",
                             allowOrder, allowReceiptOfGoods, 
                             smsArriveOfGoods, smsOrderConfirm, smsOfDelivery, 
                             emailArriveOfGoods, emailOfOrderConfirm, emailOfDelivery, 
                             webAdmin, priceListDownload, invoiceInfo, 
                             userName, password, leftCompany, newsletter, recId, refRecId)
        {
            this.Id = MongoDB.Bson.ObjectId.GenerateNewId();
        }

        [MongoDB.Bson.Serialization.Attributes.BsonId(Order = 1)]    
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public MongoDB.Bson.ObjectId Id { set; get; }

    }
}
