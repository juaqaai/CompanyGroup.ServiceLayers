using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// látogató model 
    /// </summary>
    public class Visitor //: CompanyGroup.Dto.PartnerModule.Visitor
    {
        public Visitor()
        {
            this.Id = String.Empty;
            this.CompanyId = String.Empty;
            this.CompanyName = String.Empty;
            this.PersonId = String.Empty;
            this.PersonName = String.Empty;
            this.LoggedIn = false;
            this.IsValidLogin = false;
            this.Permission = new Dto.PartnerModule.Permission();
            this.Roles = new List<string>();
            this.History = new List<string>();
            this.Representative = new Dto.PartnerModule.Representative();
            this.PaymTermId = String.Empty;
            this.Currency = "HUF";
            this.InventLocation = String.Empty;
            this.LanguageId = "HU";
            this.BscAuthorized = false;
            this.HrpAuthorized = false;
            this.ErrorMessage = String.Empty;
            this.SelectedCurrency = String.Empty;
            this.InverseLanguageId = this.LanguageId.ToUpper().Equals("HU") ? "EN" : "HU";
            this.SelectedCurrency = "HUF";
        }

        public Visitor(CompanyGroup.Dto.PartnerModule.Visitor visitor)
        {
            this.Id = visitor.Id;
            this.CompanyId = visitor.CompanyId;
            this.CompanyName = visitor.CompanyName;
            this.PersonId = visitor.PersonId;
            this.PersonName = visitor.PersonName;
            this.LoggedIn = visitor.LoggedIn;
            this.IsValidLogin = visitor.IsValidLogin;
            this.Permission = visitor.Permission;
            this.Roles = visitor.Roles;
            this.History = visitor.History;
            this.Representative = visitor.Representative;
            this.PaymTermId = visitor.PaymTermId;
            this.Currency = visitor.Currency;
            this.InventLocation = visitor.InventLocation;
            this.LanguageId = visitor.LanguageId;
            this.BscAuthorized = visitor.BscAuthorized;
            this.HrpAuthorized = visitor.HrpAuthorized;
            this.ErrorMessage = String.Empty;
            this.SelectedCurrency = visitor.Currency;
            this.InverseLanguageId = this.LanguageId.ToUpper().Equals("HU") ? "EN" : "HU";
        }

        public string ErrorMessage { get; set; }

        public string SelectedCurrency { get; set; }

        public bool CurrencyIsHuf { get { return this.Currency.ToUpper().Equals("HUF") || String.IsNullOrWhiteSpace(this.Currency); } }

        public bool CurrencyIsEur { get { return this.Currency.ToUpper().Equals("EUR"); } }

        public bool CurrencyIsUsd { get { return this.Currency.ToUpper().Equals("USD"); } }

        //public bool LanguageIsHun { get { return this.LanguageId.ToUpper().Equals("HU") || String.IsNullOrWhiteSpace(this.LanguageId); } }

        //public bool LanguageIsEng { get { return this.LanguageId.ToUpper().Equals("EN"); } }

        public string InverseLanguageId { get; set; }

        public bool PersonIdNotNullOrEmpty { get { return !String.IsNullOrWhiteSpace(this.PersonId); }  }

        public string Id { get; set; }

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
        /// belépés megtörtént-e
        /// </summary>
        public bool LoggedIn { get; set; }

        /// <summary>
        /// belépés megtörtént-e
        /// </summary>
        public bool IsValidLogin { get; set; }

        /// <summary>
        /// látogató jogosultsági beállításai
        /// </summary>
        public CompanyGroup.Dto.PartnerModule.Permission Permission { get; set; }

        /// <summary>
        /// szerepkörök
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        /// látogatott oldalak url listája
        /// </summary>
        public List<string> History { get; set; }

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
        /// képviselő
        /// </summary>
        public CompanyGroup.Dto.PartnerModule.Representative Representative { set; get; }

        /// <summary>
        /// jogosult a HRP-ben vásárolni
        /// </summary>
        public bool HrpAuthorized { get; set; }

        /// <summary>
        /// jogosult a BSC-ben vásárolni
        /// </summary>
        public bool BscAuthorized { get; set; }
    }
}
