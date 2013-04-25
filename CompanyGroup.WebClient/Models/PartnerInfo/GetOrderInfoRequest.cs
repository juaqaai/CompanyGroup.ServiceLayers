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
            this.CanBeTaken = true;

            this.SalesStatus = 1;

            this.CustomerOrderNo = String.Empty;

            this.ItemName = String.Empty;

            this.ItemId = String.Empty;

            this.SalesOrderId = String.Empty;
        }

        /// <summary>
        /// CanBeTaken
        /// </summary>
        public bool CanBeTaken { get; set; }

        /// <summary>
        /// SalesStatus
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
