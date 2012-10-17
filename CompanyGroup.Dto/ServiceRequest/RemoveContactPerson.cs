using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "RemoveContactPerson", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class RemoveContactPerson
    {
        [System.Runtime.Serialization.DataMember(Name = "RegistrationId", Order = 1)]
        public string RegistrationId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "LanguageId", Order = 2)]
        public string LanguageId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Id", Order = 3)]
        public string Id { get; set; }
    }
}
