using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// megrendelés információk lekérdezés paramétereit összefogó osztály
    /// </summary>
    public class GetOrderInfoRequest
    {
        public GetOrderInfoRequest()
        {
            this.OnOrder = true;

            this.Reserved = true;

            this.ReservedOrdered = true;
        }

        /// <summary>
        /// StatusIssue mező értéke 6 (rendelés alatt)
        /// </summary>
        public bool OnOrder { get; set; }

        /// <summary>
        /// StatusIssue mező értéke 4 (foglalt tényleges)
        /// </summary>
        public bool Reserved { get; set; }

        /// <summary>
        /// StatusIssue mező értéke 5 (foglalt rendelt)
        /// </summary>
        public bool ReservedOrdered { get; set; }
    }
}
