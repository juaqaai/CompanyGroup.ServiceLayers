using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.PartnerModule
{
    public class LoginInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="companyName"></param>
        /// <param name="personId"></param>
        /// <param name="personName"></param>
        /// <param name="email"></param>
        /// <param name="canOrder"></param>
        /// <param name="recieveGoods"></param>
        /// <param name="priceListDownloadEnabled"></param>
        /// <param name="isWebAdministrator"></param>
        /// <param name="invoiceInfoEnabled"></param>
        /// <param name="loginType"></param>
        /// <param name="partnerModel"></param>
        /// <param name="paymTermId"></param>
        /// <param name="currency"></param>
        /// <param name="inventLocation"></param>
        /// <param name="languageId"></param>
        /// <param name="priceGroup"></param>
        /// <param name="representativeId"></param>
        /// <param name="representativeName"></param>
        /// <param name="representativePhone"></param>
        /// <param name="representativeMobile"></param>
        /// <param name="representativeExtension"></param>
        /// <param name="representativeEmail"></param>
        public LoginInfo(string companyId, string companyName, string personId, string personName, string email, bool canOrder,
                         bool recieveGoods, bool priceListDownloadEnabled, bool isWebAdministrator, bool invoiceInfoEnabled, int loginType, int partnerModel, string paymTermId, string currency, string inventLocation, string languageId,
                         int priceGroup, string representativeId, string representativeName, string representativePhone, string representativeMobile, string representativeExtension, string representativeEmail)
        {
            this.CompanyId = companyId;

            this.CompanyName = companyName;

            this.PersonId = personId;

            this.PersonName = personName;

            this.Email = email;

            this.LoginType = (LoginType)loginType;

            this.SetLoggedIn(loginType);

            this.IsWebAdministrator = isWebAdministrator;

            this.InvoiceInfoEnabled = invoiceInfoEnabled;

            this.PriceListDownloadEnabled = priceListDownloadEnabled;

            this.CanOrder = canOrder;

            this.RecieveGoods = recieveGoods;

            this.PartnerModel = (PartnerModel) partnerModel;

            this.PaymTermId = paymTermId;

            this.Currency = currency;

            this.InventLocation = inventLocation;

            this.LanguageId = languageId;

            this.PriceGroup = priceGroup;

            this.RepresentativeId = representativeId;

            this.RepresentativeName = representativeName;

            this.RepresentativePhone = representativePhone;

            this.RepresentativeMobile = representativeMobile;

            this.RepresentativeExtension = representativeExtension;

            this.RepresentativeEmail = representativeEmail;
        }

        /// <summary>
        /// vállalat azonosítója
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// vállalat neve
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// személy azonosítója
        /// </summary>
        public string PersonId { get; set; }

        /// <summary>
        /// személy neve
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// bejelentkezett-e, vagy sem
        /// </summary>
        public bool LoggedIn { get; set; }

        /// <summary>
        /// bejelentkezés típusa
        /// </summary>
        public LoginType LoginType { get; set; }

        /// <summary>
        /// web adminisztrátor-e?
        /// </summary>
        public bool IsWebAdministrator { get; set; }

        /// <summary>
        /// számla info elérhető-e?
        /// </summary>
        public bool InvoiceInfoEnabled { get; set; }

        /// <summary>
        /// árlista info elérhető-e?
        /// </summary>
        public bool PriceListDownloadEnabled { get; set; }

        /// <summary>
        /// rendelés elérhető-e?
        /// </summary>
        public bool CanOrder { get; set; }

        /// <summary>
        /// árucikkek átvétele elérhető-e?
        /// </summary>
        public bool RecieveGoods { get; set; }

        /// <summary>
        /// melyik cégbe regisztrált partnerről van szó?
        /// </summary>
        public PartnerModel PartnerModel { get; set; }

        /// <summary>
        /// fizetési feltételek
        /// </summary>
        public string PaymTermId { set; get; }

        /// <summary>
        /// alapértelmezett valutanem
        /// </summary>
        public string Currency { set; get; }

        /// <summary>
        /// alapértelmezett raktár
        /// </summary>
        public string InventLocation { set; get; }

        /// <summary>
        /// alapértelmezett nyelv
        /// </summary>
        public string LanguageId { set; get; }

        /// <summary>
        /// árbesorolás 
        /// </summary>
        public int PriceGroup { set; get; }

        /// <summary>
        /// képviselő azonosító
        /// </summary>
        public string RepresentativeId	 { set; get; }

        /// <summary>
        /// képviselő neve
        /// </summary>
        public string RepresentativeName { set; get; }	

        /// <summary>
        /// képviselő telefon
        /// </summary>
        public string RepresentativePhone { set; get; }	

         /// <summary>
        /// képviselő mobil telefon
        /// </summary>
        public string RepresentativeMobile { set; get; }	

        /// <summary>
        /// képviselő mellék
        /// </summary>
        public string RepresentativeExtension { set; get; }

        /// <summary>
        /// képviselő email cím
        /// </summary>
        public string RepresentativeEmail { set; get; }

        /// <summary>
        /// bejelentkezett-e tulajdonság beállítása beállító metóduson keresztül       
        /// </summary>
        /// <param name="loginType"></param>
        private void SetLoggedIn(int loginType)
        {
            bool personalLoginOK = (loginType == 2) && (!String.IsNullOrWhiteSpace(this.CompanyId)) && (!String.IsNullOrWhiteSpace(this.CompanyName)) && (!String.IsNullOrWhiteSpace(this.PersonId)) && (!String.IsNullOrWhiteSpace(this.PersonName));

            bool companyLoginOK = (loginType == 1) && (!String.IsNullOrWhiteSpace(this.CompanyId)) && (!String.IsNullOrWhiteSpace(this.CompanyName));

            this.LoggedIn = (loginType > 0) && (personalLoginOK || companyLoginOK);
        }


    }
}
