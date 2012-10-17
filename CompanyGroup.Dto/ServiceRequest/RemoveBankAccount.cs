using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{

    [System.Runtime.Serialization.DataContract(Name = "RemoveBankAccount", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class RemoveBankAccount
    {
        [System.Runtime.Serialization.DataMember(Name = "RegistrationId", Order = 1)]
        public string RegistrationId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "LanguageId", Order = 2)]
        public string LanguageId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "BankAccountId", Order = 3)]
        public string BankAccountId { get; set; }
    }
}
