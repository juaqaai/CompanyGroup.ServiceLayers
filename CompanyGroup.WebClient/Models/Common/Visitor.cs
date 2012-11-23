using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// látogató model 
    /// </summary>
    public class Visitor : CompanyGroup.Dto.PartnerModule.Visitor
    {
        public Visitor() : base() 
        {
            this.ErrorMessage = String.Empty;
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
        }

        public string ErrorMessage { get; set; }

        public string SelectedCurrency { get; set; }

        public bool CurrencyIsHuf { get { return this.Currency.Equals("HUF") || String.IsNullOrWhiteSpace(this.Currency); } }

        public bool CurrencyIsEur { get { return this.Currency.Equals("EUR"); } }

        public bool CurrencyIsUsd { get { return this.Currency.Equals("USD"); } }

        public bool LanguageIsHun { get { return this.LanguageId.Equals("hun") || String.IsNullOrWhiteSpace(this.LanguageId); } }

        public bool LanguageIsEng { get { return this.LanguageId.Equals("eng"); } }

        public bool PersonIdNotNullOrEmpty { get { return !String.IsNullOrWhiteSpace(this.PersonId); }  }
    }
}
