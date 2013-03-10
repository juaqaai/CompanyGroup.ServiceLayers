using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    public class GetRegistrationByKey
    {
        public GetRegistrationByKey(string id, string visitorId)
        {
            this.Id = id;

            this.VisitorId = visitorId;
        }

        public string Id { get; set; }

        public string VisitorId { get; set; }
    }
}
