using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// megrendelés információk lekérdezés paramétereit összefogó típus
    /// StatusIssue: 0 none, 1 sold, 2 deducted (eladva), 3 picked (kivéve), 4 ReservPhysical (foglalt tényleges), 5 ReservOrdered (foglalt rendelt), 6 OnOrder (rendelés alatt), 7 Quotation issue (árajánlat kiadása)
    /// </summary>
    public class GetOrderInfoRequest
    {
        public GetOrderInfoRequest() : this("", "", true, true, true) {}

        public GetOrderInfoRequest(string visitorId, string languageId, bool onOrder, bool reserved, bool reservedOrdered)
        {
            this.VisitorId = visitorId;

            this.LanguageId = languageId;

            this.OnOrder = onOrder;

            this.Reserved = reserved;

            this.ReservedOrdered = reservedOrdered;
        }

        public string VisitorId { get; set; }

        public string LanguageId { get; set; }

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
