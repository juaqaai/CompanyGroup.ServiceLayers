using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "GetRegistrationByKey", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class GetRegistrationByKey
    {
        [System.Runtime.Serialization.DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }
    }
}
