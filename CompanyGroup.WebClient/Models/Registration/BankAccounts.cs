using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class BankAccounts
    {

        public BankAccounts(CompanyGroup.Dto.RegistrationModule.BankAccounts bankAccounts)
        {
            this.Items = bankAccounts.Items.ConvertAll(x => new CompanyGroup.WebClient.Models.BankAccount(x));

            SelectedId = String.Empty;
        }

        /// <summary>
        /// módosításra kiválasztott bankszámla azonosító
        /// </summary>
        public string SelectedId { get; set; }

        public List<CompanyGroup.WebClient.Models.BankAccount> Items { get; set; }
    }
}
