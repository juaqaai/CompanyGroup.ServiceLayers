using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class ContactPerson
    {
        public ContactPerson(CompanyGroup.Dto.RegistrationModule.ContactPerson contactPerson, string selectedId)
        {
            this.AllowOrder = contactPerson.AllowOrder;
            this.AllowReceiptOfGoods = contactPerson.AllowReceiptOfGoods;
            this.ContactPersonId = contactPerson.ContactPersonId;
            this.Email = contactPerson.Email;
            this.EmailArriveOfGoods = contactPerson.EmailArriveOfGoods;
            this.EmailOfDelivery = contactPerson.EmailOfDelivery;
            this.EmailOfOrderConfirm = contactPerson.EmailOfOrderConfirm;
            this.FirstName = contactPerson.FirstName;
            this.Id = contactPerson.Id; 
            this.InvoiceInfo = contactPerson.InvoiceInfo;
            this.LastName = contactPerson.LastName;
            this.LeftCompany = contactPerson.LeftCompany;
            this.Newsletter = contactPerson.Newsletter;
            this.Password = contactPerson.Password;
            this.PriceListDownload = contactPerson.PriceListDownload;
            this.RecId = contactPerson.RecId;
            //this.RefRecId = contactPerson.RefRecId;
            this.SmsArriveOfGoods = contactPerson.SmsArriveOfGoods;
            this.SmsOfDelivery = contactPerson.SmsOfDelivery;
            this.SmsOrderConfirm = contactPerson.SmsOrderConfirm;
            this.Telephone = contactPerson.Telephone;
            this.UserName = contactPerson.UserName;
            this.WebAdmin = contactPerson.WebAdmin;
            this.Positions = contactPerson.Positions;
            this.SelectedItem = this.Id.Equals(selectedId);
        }

        public ContactPerson() : this(new CompanyGroup.Dto.RegistrationModule.ContactPerson(), String.Empty){ }

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

        public long RecId{ set; get; }

        /// <summary>
        /// megadott pozíciók
        /// </summary>
        public List<string> Positions { get; set; }

        /// <summary>
        /// szerkesztésre kiválasztott 
        /// </summary>
        public bool SelectedItem { get; set; }
    }
}
