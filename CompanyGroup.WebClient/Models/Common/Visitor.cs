﻿using System;
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
        }

        public string ErrorMessage { get; set; }
    }
}
