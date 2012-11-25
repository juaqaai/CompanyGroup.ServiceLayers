using System;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "GetDeliveryAddresses", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class GetDeliveryAddresses
    {
        public GetDeliveryAddresses() : this(String.Empty, String.Empty) { } 

        public GetDeliveryAddresses(string dataAreaId, string visitorId)
        {
            this.DataAreaId = dataAreaId;

            this.VisitorId = visitorId;
        }

        [System.Runtime.Serialization.DataMember(Name = "DataAreaId", Order = 1)]
        public string DataAreaId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "VisitorId", Order = 2)]
        public string VisitorId { get; set; }
    }
}
