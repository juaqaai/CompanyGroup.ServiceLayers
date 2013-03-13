using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class BankAccount
    {
        public BankAccount(CompanyGroup.Dto.RegistrationModule.BankAccount bankAccount, string selectedId)
        {
            this.Id = bankAccount.Id;
            this.Part1 = bankAccount.Part1;
            this.Part2 = bankAccount.Part2;
            this.Part3 = bankAccount.Part3;
            this.RecId = bankAccount.RecId;
            this.SelectedItem = (selectedId.Equals(bankAccount.Id));
        }

        public BankAccount() : this(new CompanyGroup.Dto.RegistrationModule.BankAccount(), "") { }

        public string Part1 { set; get; }

        public string Part2 { set; get; }

        public string Part3 { set; get; }

        public long RecId { set; get; }

        public string Id { set; get; }

        public bool SelectedItem { get; set; }
    }
}