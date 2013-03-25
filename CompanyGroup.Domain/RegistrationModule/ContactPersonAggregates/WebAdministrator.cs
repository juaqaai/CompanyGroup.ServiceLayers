using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.RegistrationModule
{
    public class WebAdministrator : CompanyGroup.Domain.PartnerModule.ContactPerson
    {
        public WebAdministrator() : this("", "", "", "", "", "", false, false, false, false, false, false, false, false, false, false, "", "", false, false, 0, 0) { }

        public WebAdministrator(string contactPersonId, string lastName, string firstName, string email, string cellularPhone, string phone, 
                             bool allowOrder, bool allowReceiptOfGoods, 
                             bool smsArriveOfGoods, bool smsOrderConfirm, bool smsOfDelivery, 
                             bool emailArriveOfGoods, bool emailOfOrderConfirm, bool emailOfDelivery, 
                             bool priceListDownload, bool invoiceInfo,
                             string userName, string password, bool leftCompany, bool newsletter, long recId, long refRecId)
            : base(contactPersonId, lastName, firstName, email, cellularPhone, phone, "", 
                             allowOrder, allowReceiptOfGoods, 
                             smsArriveOfGoods, smsOrderConfirm, smsOfDelivery, 
                             emailArriveOfGoods, emailOfOrderConfirm, emailOfDelivery, 
                             true, priceListDownload, invoiceInfo, 
                             userName, password, leftCompany, newsletter, recId, refRecId) { }
    }
}
