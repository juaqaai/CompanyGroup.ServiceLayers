using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// vevőregisztráció lekérdezés paramétereit összefogó típus
    /// </summary>
    public class GetCustomerRegistrationRequest
    {
        public GetCustomerRegistrationRequest(string visitorId, string dataAreaId)
        {
            this.VisitorId = visitorId;

            this.DataAreaId = dataAreaId;
        }

        public GetCustomerRegistrationRequest() : this("", "") { }

        public string DataAreaId { get; set; }

        public string VisitorId { get; set; }
    }
}
