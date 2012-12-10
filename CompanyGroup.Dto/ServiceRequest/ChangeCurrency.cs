using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.Dto.ServiceRequest
{
    public class ChangeCurrency
    {
        public ChangeCurrency() : this(String.Empty, String.Empty) { }

        public ChangeCurrency(string visitorId, string currency)
        {
            VisitorId = visitorId;

            Currency = currency;
        }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        public string VisitorId { get; set; }

        /// <summary>
        /// választott nyelv
        /// </summary>
        public string Currency { get; set; }
    }
}
