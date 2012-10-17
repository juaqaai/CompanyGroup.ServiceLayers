using System;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "GetDeliveryAddresses", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class GetDeliveryAddresses
    {
        [System.Runtime.Serialization.DataMember(Name = "DataAreaId", Order = 1)]
        public string DataAreaId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "VisitorId", Order = 2)]
        public string VisitorId { get; set; }
    }
}
