using System;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "GetContactPerson", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class GetContactPerson
    {
        [System.Runtime.Serialization.DataMember(Name = "RegistrationId", Order = 1)]
        public string RegistrationId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "LanguageId", Order = 3)]
        public string LanguageId { get; set; }
    }
}
