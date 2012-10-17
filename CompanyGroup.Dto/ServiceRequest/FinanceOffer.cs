using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "FinanceOffer", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class FinanceOffer
    {
        [System.Runtime.Serialization.DataMember(Name = "VisitorId", Order = 1)]
        public string VisitorId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "PersonName", Order = 2)]
        public string PersonName { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Address", Order = 3)]
        public string Address { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Phone", Order = 4)]
        public string Phone { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "StatNumber", Order = 5)]
        public string StatNumber { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "NumOfMonth", Order = 6)]
        public int NumOfMonth { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "CartId", Order = 7)]
        public string CartId { get; set; }
    }
}
