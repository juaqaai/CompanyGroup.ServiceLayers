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
        public GetOrderInfoRequest() : this(String.Empty, String.Empty, true, 0, String.Empty, String.Empty, String.Empty, String.Empty) { }

        public GetOrderInfoRequest(string visitorId, string languageId, bool canBeTaken, int salesStatus, string customerOrderNo, string itemName, string itemId, string salesOrderId)
        {
            this.VisitorId = visitorId;

            this.LanguageId = languageId;

            this.CanBeTaken = canBeTaken;

            this.SalesStatus = salesStatus;

            this.CustomerOrderNo = customerOrderNo;

            this.ItemName = itemName;

            this.ItemId = itemId;

            this.SalesOrderId = salesOrderId;
        }

        public string VisitorId { get; set; }

        public string LanguageId { get; set; }

        /// <summary>
        /// StatusIssue mező értéke 6 (rendelés alatt)
        /// </summary>
        public bool CanBeTaken { get; set; }

        /// <summary>
        /// 1: nyitott rendelés
        /// </summary>
        public int SalesStatus { get; set; }

        /// <summary>
        /// vevő saját rendelés azonosító a vevő nyilvántartásában
        /// </summary>
        public string CustomerOrderNo { get; set; }

        /// <summary>
        /// cikk neve
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// cikk azonosítója
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// megrendelés azonosító
        /// </summary>
        public string SalesOrderId { get; set; }

    }
}
