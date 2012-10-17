using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    [System.Runtime.Serialization.DataContract(Name = "BankAccount", Namespace = "CompanyGroup.Dto.PartnerModule")]
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
}
