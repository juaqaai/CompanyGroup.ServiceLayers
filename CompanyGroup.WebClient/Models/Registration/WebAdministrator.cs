using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// webadminisztrátor adatok
    /// </summary>
    public class WebAdministrator : CompanyGroup.Dto.RegistrationModule.WebAdministrator
    {
        public WebAdministrator(CompanyGroup.Dto.RegistrationModule.WebAdministrator webAdministrator)
        {
            this.AllowOrder = webAdministrator.AllowOrder;
            this.AllowReceiptOfGoods = webAdministrator.AllowReceiptOfGoods;
            this.ContactPersonId = webAdministrator.ContactPersonId;
            this.Email = webAdministrator.Email;
            this.EmailArriveOfGoods = webAdministrator.EmailArriveOfGoods;
            this.EmailOfDelivery = webAdministrator.EmailOfDelivery;
            this.EmailOfOrderConfirm = webAdministrator.EmailOfOrderConfirm;
            this.FirstName = webAdministrator.FirstName;
            this.InvoiceInfo = webAdministrator.InvoiceInfo;
            this.LastName = webAdministrator.LastName;
            this.LeftCompany = webAdministrator.LeftCompany;
            this.Newsletter = webAdministrator.Newsletter;
            this.Password = webAdministrator.Password;
            this.PriceListDownload = webAdministrator.PriceListDownload;
            this.RecId = webAdministrator.RecId;
            this.RefRecId = webAdministrator.RefRecId;
            this.SmsArriveOfGoods = webAdministrator.SmsArriveOfGoods;
            this.SmsOfDelivery = webAdministrator.SmsOfDelivery;
            this.SmsOrderConfirm = webAdministrator.SmsOrderConfirm;
            this.Telephone = webAdministrator.Telephone;
            this.UserName = webAdministrator.UserName;
        }

        public WebAdministrator() { }
    }

    public class RemoveContactPerson
    {
        public string Id { get; set; }
    }

    public class RemoveDeliveryAddress
    {
        public string Id { get; set; }
    }

    public class SelectForUpdateBankAccount
    {
        /// <summary>
        /// módosításra kiválasztott bankszámla azonosító
        /// </summary>
        public string SelectedId { get; set; }
    }

    public class SelectForUpdateDeliveryAddress
    {
        /// <summary>
        /// módosításra kiválasztott szállítási cím azonosító
        /// </summary>
        public string SelectedId { get; set; }
    }

    public class SelectForUpdateContactPerson
    {
        /// <summary>
        /// módosításra kiválasztott kapcsolattart= azonosító
        /// </summary>
        public string SelectedId { get; set; }
    }
}
