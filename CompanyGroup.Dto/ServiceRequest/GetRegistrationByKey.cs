using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "GetRegistrationByKey", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class GetRegistrationByKey
    {
        public GetRegistrationByKey(string id, string visitorId)
        {
            this.Id = id;

            this.VisitorId = visitorId;
        }

        [System.Runtime.Serialization.DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "VisitorId", Order = 2)]
        public string VisitorId { get; set; }
    }
}
