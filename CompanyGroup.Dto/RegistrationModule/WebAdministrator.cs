using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.RegistrationModule
{
    /// <summary>
    /// webadminisztrátor adatok DTO
    /// </summary>
    public class WebAdministrator
    {
        public WebAdministrator(bool allowOrder, bool allowReceiptOfGoods, string contactPersonId, string email, bool emailArriveOfGoods, bool emailOfDelivery, bool emailOfOrderConfirm,
                                string firstName, bool invoiceInfo, string lastName, bool leftCompany, bool newsletter, string password, bool priceListDownload, long recId,
                                long refRecId, bool smsArriveOfGoods, bool smsOfDelivery, bool smsOrderConfirm, string telephone, string userName, List<string> positions)
        {
            this.AllowOrder = allowOrder;
            this.AllowReceiptOfGoods = allowReceiptOfGoods;
            this.ContactPersonId = contactPersonId;
            this.Email = email;
            this.EmailArriveOfGoods = emailArriveOfGoods;
            this.EmailOfDelivery = emailOfDelivery;
            this.EmailOfOrderConfirm = emailOfOrderConfirm;
            this.FirstName = firstName;
            this.InvoiceInfo = invoiceInfo;
            this.LastName = lastName;
            this.LeftCompany = leftCompany;
            this.Newsletter = newsletter;
            this.Password = password;
            this.PriceListDownload = priceListDownload;
            this.RecId = recId;
            this.RefRecId = refRecId;
            this.SmsArriveOfGoods = smsArriveOfGoods;
            this.SmsOfDelivery = smsOfDelivery;
            this.SmsOrderConfirm = smsOrderConfirm;
            this.Telephone = telephone;
            this.UserName = userName;
            this.Positions = positions;
        }

        public WebAdministrator() : this(false, false, String.Empty, String.Empty, false, false, false, String.Empty, false, String.Empty, false, false, String.Empty, false, 0, 0, false, false, false, String.Empty, String.Empty, new List<string>()) { }

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

        /// <summary>
        /// megadott pozíciók
        /// </summary>
        public List<string> Positions { get; set; }
    }


}
