using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "AddContactPerson", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class AddContactPerson
    {
        [System.Runtime.Serialization.DataMember(Name = "RegistrationId", Order = 1)]
        public string RegistrationId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "LanguageId", Order = 3)]
        public string LanguageId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ContactPerson", Order = 2)]
        public CompanyGroup.Dto.RegistrationModule.ContactPerson ContactPerson { get; set; }
    }
}
