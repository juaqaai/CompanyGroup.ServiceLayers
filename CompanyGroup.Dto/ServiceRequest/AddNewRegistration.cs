using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "AddNewRegistration", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class AddNewRegistration
    {
        [System.Runtime.Serialization.DataMember(Name = "VisitorId", Order = 1)]
        public string VisitorId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "LanguageId", Order = 2)]
        public string LanguageId { get; set; }
    }
}
