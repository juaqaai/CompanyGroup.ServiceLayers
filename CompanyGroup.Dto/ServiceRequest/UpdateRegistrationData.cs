using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "UpdateRegistrationData", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class UpdateRegistrationData
    {
        [System.Runtime.Serialization.DataMember(Name = "RegistrationId", Order = 1)]
        public string RegistrationId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "LanguageId", Order = 2)]
        public string LanguageId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "CompanyData", Order = 3)]
        public CompanyGroup.Dto.RegistrationModule.CompanyData CompanyData { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "InvoiceAddress", Order = 4)]                                
        public CompanyGroup.Dto.RegistrationModule.InvoiceAddress InvoiceAddress { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "MailAddress", Order = 5)]
        public CompanyGroup.Dto.RegistrationModule.MailAddress MailAddress { get; set; } 
    }
}
