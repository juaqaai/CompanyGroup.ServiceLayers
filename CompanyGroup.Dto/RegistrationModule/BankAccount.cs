using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.RegistrationModule
{
    public class BankAccount
    {
        public BankAccount()
        {
            this.Part1 = String.Empty;
            this.Part2 = String.Empty;
            this.Part3 = String.Empty;
            this.RecId = 0;
            this.Id = String.Empty;
        }

        public BankAccount(string part1, string part2, string part3, long recId, string id)
        {
            this.Part1 = part1;
            this.Part2 = part2;
            this.Part3 = part3;
            this.RecId = recId;
            this.Id = id;
        }

        public string Part1 { set; get; }

        public string Part2 { set; get; }

        public string Part3 { set; get; }

        public long RecId { set; get; }

         public string Id { set; get; }
    }

    public class BankAccounts
    {
        public List<CompanyGroup.Dto.RegistrationModule.BankAccount> Items { get; set; }

        public BankAccounts()
        {
            this.Items = new List<CompanyGroup.Dto.RegistrationModule.BankAccount>();
        }

        public BankAccounts(List<CompanyGroup.Dto.RegistrationModule.BankAccount> items)
        {
            this.Items = items;
        }
    }
}
