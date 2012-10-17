using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "UpdateContactPerson", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class UpdateContactPerson
    {
        [System.Runtime.Serialization.DataMember(Name = "RegistrationId", Order = 1)]
        public string RegistrationId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "LanguageId", Order = 2)]
        public string LanguageId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ContactPerson", Order = 3)]
        public CompanyGroup.Dto.RegistrationModule.ContactPerson ContactPerson { get; set; }
    }
}
