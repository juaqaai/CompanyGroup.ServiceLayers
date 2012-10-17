using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "GetOrderInfo", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class GetOrderInfo
    {
        [System.Runtime.Serialization.DataMember(Name = "VisitorId", Order = 1)]
        public string VisitorId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "LanguageId", Order = 2)]
        public string LanguageId { get; set; }
    }
}
