using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// webadminisztrátor adatok
    /// </summary>
    public class WebAdministrator
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

        /// <summary>
        /// kapcsolattartó egyedi azonosító
        /// </summary>
        public string ContactPersonId { get; set; }

        /// <summary>
        /// kapcsolattartó vezetékneve
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///kapcsolattartó keresztneve
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// engedélyezett-e a rendelés?
        /// </summary>
        public bool AllowOrder { get; set; }

        /// <summary>
        /// engedélyezett-e az áru átvétele?
        /// </summary>
        public bool AllowReceiptOfGoods { get; set; }

        /// <summary>
        /// sms küldés be van-e állítva az árubeérkezésről?
        /// </summary>
        public bool SmsArriveOfGoods { get; set; }

        /// <summary>
        /// sms küldés be van-e állítva a megrendelésről?
        /// </summary>
        public bool SmsOrderConfirm { get; set; }

        /// <summary>
        /// sms küldés be van-e állítva a kiszállításról?
        /// </summary>
        public bool SmsOfDelivery { get; set; }

        /// <summary>
        /// email küldés be van-e állítva az árubeérkezésről?
        /// </summary>
        public bool EmailArriveOfGoods { get; set; }

        /// <summary>
        /// email küldés be van-e állítva a megrendelésről?
        /// </summary>
        public bool EmailOfOrderConfirm { get; set; }

        /// <summary>
        /// email küldés be van-e állítva a kiszállításról?
        /// </summary>
        public bool EmailOfDelivery { get; set; }

        /// <summary>
        /// árlistát letölthet-e?
        /// </summary>
        public bool PriceListDownload { get; set; }

        /// <summary>
        /// számlainformációt nézhet-e?
        /// </summary>
        public bool InvoiceInfo { get; set; }

        /// <summary>
        /// webes belépési név
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// webes belépési jelszó
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// hírlevél beállítás érvényben, van-e? 
        /// </summary>
        public bool Newsletter { get; set; }

        /// <summary>
        /// mobil, vagy vonalas telefonszám 
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// kapcsolattartó email cím
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// törölt?
        /// </summary>
        public bool LeftCompany { get; set; }

        /// <summary>
        /// rec id
        /// </summary>
        public long RecId { get; set; }

        /// <summary>
        /// refRec id
        /// </summary>
        public long RefRecId { get; set; }
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
