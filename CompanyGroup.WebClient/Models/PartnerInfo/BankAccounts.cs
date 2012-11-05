using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class BankAccounts : CompanyGroup.Dto.RegistrationModule.BankAccounts
    {
        public BankAccounts() : base()
        {
            SelectedId = String.Empty;
        }

        /// <summary>
        /// módosításra kiválasztott bankszámla azonosító
        /// </summary>
        public string SelectedId { get; set; }
    }
}
