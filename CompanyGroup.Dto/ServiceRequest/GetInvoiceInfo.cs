using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "GetInvoiceInfo", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class GetInvoiceInfo
    {
        [System.Runtime.Serialization.DataMember(Name = "VisitorId", Order = 1)]
        public string VisitorId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "LanguageId", Order = 2)]
        public string LanguageId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "PaymentType", Order = 3)]
        public int PaymentType { get; set; }
    }
}
