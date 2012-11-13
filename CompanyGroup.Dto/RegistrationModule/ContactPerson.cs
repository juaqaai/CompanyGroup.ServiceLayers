using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.RegistrationModule
{
    [System.Runtime.Serialization.DataContract(Name = "ContactPerson", Namespace = "CompanyGroup.Dto.RegistrationModule")]
    public class ContactPerson
    {
        public ContactPerson() : this("", "", "", false, false, false, false, false, false, false, false, false, false, false, "", "", false, "", "", false, "") { }

        public ContactPerson(string contactPersonId, string firstName, string lastName,
                             bool allowOrder, bool allowReceiptOfGoods, bool smsArriveOfGoods, bool smsOrderConfirm, bool smsOfDelivery,
                             bool emailArriveOfGoods, bool emailOfOrderConfirm, bool emailOfDelivery, bool webAdmin, bool priceListDownload, bool invoiceInfo,
                             string userName, string password, bool newsletter, string telephone, string email, bool leftCompany, string id)
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
        }

        /// <summary>
        /// kapcsolattartó egyedi azonosító
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "ContactPersonId", Order = 1)]
        public string ContactPersonId { get; set; }

        /// <summary>
        /// kapcsolattartó vezetékneve
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "FirstName", Order = 2)]
        public string FirstName { get; set; }

        /// <summary>
        ///kapcsolattartó keresztneve
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "LastName", Order = 3)]
        public string LastName { get; set; }

        /// <summary>
        /// engedélyezett-e a rendelés?
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "AllowOrder", Order = 4)]
        public bool AllowOrder { get; set; }

        /// <summary>
        /// engedélyezett-e az áru átvétele?
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "AllowReceiptOfGoods", Order = 5)]
        public bool AllowReceiptOfGoods { get; set; }

        /// <summary>
        /// sms küldés be van-e állítva az árubeérkezésről?
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "SmsArriveOfGoods", Order = 6)]
        public bool SmsArriveOfGoods { get; set; }

        /// <summary>
        /// sms küldés be van-e állítva a megrendelésről?
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "SmsOrderConfirm", Order = 7)]
        public bool SmsOrderConfirm { get; set; }

        /// <summary>
        /// sms küldés be van-e állítva a kiszállításról?
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "SmsOfDelivery", Order = 8)]
        public bool SmsOfDelivery { get; set; }

        /// <summary>
        /// email küldés be van-e állítva az árubeérkezésről?
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "EmailArriveOfGoods", Order = 8)]
        public bool EmailArriveOfGoods { get; set; }

        /// <summary>
        /// email küldés be van-e állítva a megrendelésről?
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "EmailOfOrderConfirm", Order = 10)]
        public bool EmailOfOrderConfirm { get; set; }

        /// <summary>
        /// email küldés be van-e állítva a kiszállításról?
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "EmailOfDelivery", Order = 11)]
        public bool EmailOfDelivery { get; set; }

        /// <summary>
        /// Webadmin-e a kapcsolattartó?
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "WebAdmin", Order = 12)]
        public virtual bool WebAdmin { get; set; }

        /// <summary>
        /// árlistát letölthet-e?
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "PriceListDownload", Order = 13)]
        public bool PriceListDownload { get; set; }

        /// <summary>
        /// számlainformációt nézhet-e?
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "InvoiceInfo", Order = 14)]
        public bool InvoiceInfo { get; set; }

        /// <summary>
        /// webes belépési név
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "UserName", Order = 15)]
        public string UserName { get; set; }

        /// <summary>
        /// webes belépési jelszó
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Password", Order = 16)]
        public string Password { get; set; }

        /// <summary>
        /// hírlevél beállítás érvényben, van-e? 
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Newsletter", Order = 17)]
        public bool Newsletter { get; set; }

        /// <summary>
        /// mobil, vagy vonalas telefonszám 
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Telephone", Order = 18)]
        public string Telephone { get; set; }

        /// <summary>
        /// kapcsolattartó email cím
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Email", Order = 19)]
        public string Email { get; set; }

        /// <summary>
        /// törölt?
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "LeftCompany", Order = 20)]
        public bool LeftCompany { get; set; }

        /// <summary>
        /// azonosító
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Id", Order = 21)]
        public string Id { set; get; }
    }

    [System.Runtime.Serialization.DataContract(Name = "ContactPersons", Namespace = "CompanyGroup.Dto.RegistrationModule")]
    public class ContactPersons
    {
        [System.Runtime.Serialization.DataMember(Name = "Items", Order = 1)]
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
