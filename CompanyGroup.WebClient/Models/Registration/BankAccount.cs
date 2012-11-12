using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class BankAccount : CompanyGroup.Dto.RegistrationModule.BankAccount
    {
        public BankAccount(CompanyGroup.Dto.RegistrationModule.BankAccount bankAccount)
        {
            this.Id = bankAccount.Id;
            this.Part1 = bankAccount.Part1;
            this.Part2 = bankAccount.Part2;
            this.Part3 = bankAccount.Part3;
            this.RecId = bankAccount.RecId;
        }

        public BankAccount() : base() { }
    }
}