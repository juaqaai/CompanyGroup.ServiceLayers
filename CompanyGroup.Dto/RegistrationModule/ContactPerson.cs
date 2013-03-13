using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.RegistrationModule
{
    public class ContactPerson
    {
        public ContactPerson() : this("", "", "", false, false, false, false, false, false, false, false, false, false, false, "", "", false, "", "", false, "", 0) { }

        public ContactPerson(string contactPersonId, string firstName, string lastName,
                             bool allowOrder, bool allowReceiptOfGoods, bool smsArriveOfGoods, bool smsOrderConfirm, bool smsOfDelivery,
                             bool emailArriveOfGoods, bool emailOfOrderConfirm, bool emailOfDelivery, bool webAdmin, bool priceListDownload, bool invoiceInfo,
                             string userName, string password, bool newsletter, string telephone, string email, bool leftCompany, string id, long recId)
        {
            this.ContactPersonId = contactPersonId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.AllowOrder = allowOrder;
            this.AllowReceiptOfGoods = allowReceiptOfGoods;
            this.SmsArriveOfGoods = smsArriveOfGoods;
            this.SmsOrderConfirm = smsOrderConfirm;
            this.SmsOfDelivery = smsOfDelivery;
            this.EmailArriveOfGoods = emailArriveOfGoods;
            this.EmailOfOrderConfirm = emailOfOrderConfirm;
            this.EmailOfDelivery = emailOfDelivery;
            this.WebAdmin = webAdmin;
            this.PriceListDownload = priceListDownload;
            this.InvoiceInfo = invoiceInfo;
            this.UserName = userName;
            this.Password = password;
            this.Newsletter = newsletter;
            this.Telephone = telephone;
            this.Email = email;
            this.LeftCompany = leftCompany;
            this.Id = id;
            this.RecId = recId;
        }

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
        /// Webadmin-e a kapcsolattartó?
        /// </summary>
        public virtual bool WebAdmin { get; set; }

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
        /// azonosító
        /// </summary>
        public string Id { set; get; }

        /// <summary>
        /// AX rekord azonosító
        /// </summary>
        public long RecId { set; get; }
    }

    public class ContactPersons
    {
        public List<CompanyGroup.Dto.RegistrationModule.ContactPerson> Items { get; set; }

        public ContactPersons()
        {
            this.Items = new List<CompanyGroup.Dto.RegistrationModule.ContactPerson>();
        }

        public ContactPersons(List<CompanyGroup.Dto.RegistrationModule.ContactPerson> items)
        {
            this.Items = items;
        }
    }
}
