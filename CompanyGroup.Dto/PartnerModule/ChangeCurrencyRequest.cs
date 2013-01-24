using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.Dto.PartnerModule
{
    public class ChangeCurrencyRequest
    {
        public ChangeCurrencyRequest() : this(String.Empty, String.Empty) { }

        public ChangeCurrencyRequest(string visitorId, string currency)
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
