using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Dto.ServiceRequest
{
    public class GetCustomerRegistration
    {
        public GetCustomerRegistration(string visitorId, string dataAreaId)
        {
            this.VisitorId = visitorId;

            this.DataAreaId = dataAreaId;
        }

        public string DataAreaId { get; set; }

        public string VisitorId { get; set; }
    }
}
