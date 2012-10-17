using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "UpdateWebAdministrator", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class UpdateWebAdministrator
    {
        [System.Runtime.Serialization.DataMember(Name = "RegistrationId", Order = 1)]
        public string RegistrationId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "LanguageId", Order = 2)]
        public string LanguageId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "WebAdministrator", Order = 3)]
        public CompanyGroup.Dto.RegistrationModule.WebAdministrator WebAdministrator { get; set; }
    }
}
