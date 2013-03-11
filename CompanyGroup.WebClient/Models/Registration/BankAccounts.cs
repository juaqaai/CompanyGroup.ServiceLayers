using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class BankAccounts
    {

        public BankAccounts(CompanyGroup.Dto.RegistrationModule.BankAccounts bankAccounts, string selectedId)
        {
            this.Items = bankAccounts.Items.ConvertAll(x => new CompanyGroup.WebClient.Models.BankAccount(x, selectedId));
        }

        public List<CompanyGroup.WebClient.Models.BankAccount> Items { get; set; }
    }
}
