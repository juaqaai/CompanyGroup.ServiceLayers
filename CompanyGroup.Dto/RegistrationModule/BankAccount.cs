using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.RegistrationModule
{
    [System.Runtime.Serialization.DataContract(Name = "BankAccount", Namespace = "CompanyGroup.Dto.RegistrationModule")]
    public class BankAccount
    {
        [System.Runtime.Serialization.DataMember(Name = "Part1", Order = 1)]
        public string Part1 { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "Part2", Order = 2)]
        public string Part2 { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "Part3", Order = 3)]
        public string Part3 { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "RecId", Order = 4)]
        public long RecId { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "Id", Order = 5)]
        public string Id { set; get; }
    }

    [System.Runtime.Serialization.DataContract(Name = "BankAccounts", Namespace = "CompanyGroup.Dto.RegistrationModule")]
    public class BankAccounts
    {
        [System.Runtime.Serialization.DataMember(Name = "Items", Order = 1)]
        public List<CompanyGroup.Dto.RegistrationModule.BankAccount> Items { get; set; }

        public BankAccounts()
        {
            this.Items = new List<CompanyGroup.Dto.RegistrationModule.BankAccount>();
        }
    }
}
