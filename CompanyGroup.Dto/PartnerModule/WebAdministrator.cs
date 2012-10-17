using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Dto.PartnerModule
{
    [System.Runtime.Serialization.DataContract(Name = "WebAdministrator", Namespace = "CompanyGroup.Dto.PartnerModule")]
    public class WebAdministrator
    {
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
        [System.Runtime.Serialization.DataMember(Name = "EmailArriveOfGoods", Order = 9)]
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
        /// árlistát letölthet-e?
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "PriceListDownload", Order = 12)]
        public bool PriceListDownload { get; set; }

        /// <summary>
        /// számlainformációt nézhet-e?
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "InvoiceInfo", Order = 13)]
        public bool InvoiceInfo { get; set; }

        /// <summary>
        /// webes belépési név
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "UserName", Order = 14)]
        public string UserName { get; set; }

        /// <summary>
        /// webes belépési jelszó
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Password", Order = 15)]
        public string Password { get; set; }

        /// <summary>
        /// hírlevél beállítás érvényben, van-e? 
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Newsletter", Order = 16)]
        public bool Newsletter { get; set; }

        /// <summary>
        /// mobil, vagy vonalas telefonszám 
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Telephone", Order = 17)]
        public string Telephone { get; set; }

        /// <summary>
        /// kapcsolattartó email cím
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Email", Order = 18)]
        public string Email { get; set; }

        /// <summary>
        /// törölt?
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "LeftCompany", Order = 19)]
        public bool LeftCompany { get; set; }
    }
}
