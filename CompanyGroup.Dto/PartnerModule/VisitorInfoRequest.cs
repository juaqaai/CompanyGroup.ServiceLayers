using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    public class VisitorInfoRequest
    {
        public string DataAreaId { get; set; }

        public string VisitorId { get; set; }

        public VisitorInfoRequest(string visitorId, string dataAreaId)
        {
            this.VisitorId = visitorId;

            this.DataAreaId = dataAreaId;
        }

        public VisitorInfoRequest() : this("", "") { }
    }
}
