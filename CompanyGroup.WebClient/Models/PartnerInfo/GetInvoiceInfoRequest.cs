using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// számlainformációk lekérdezés paramétereit összefogó típus
    /// </summary>
    public class GetInvoiceInfoRequest
    {
        public GetInvoiceInfoRequest()
        {
            this.Debit = true;

            this.Overdue = true;

            this.ItemId = String.Empty;

            this.ItemName = String.Empty;

            this.SalesId = String.Empty;

            this.SerialNumber = String.Empty;

            this.InvoiceId = String.Empty;

            this.DateIntervall = 0;

            this.Sequence = 0;

            this.CurrentPageIndex = 1;

            this.ItemsOnPage = 30;
        }

        /// <summary>
        /// kifizetetlen - tartozás
        /// </summary>
        public bool Debit { get; set; }

        /// <summary>
        /// lejárt - határidőn túli
        /// </summary>
        public bool Overdue { get; set; }

        /// <summary>
        /// termékazonosító
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// cikk neve
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// vevőrendelés száma
        /// </summary>
        public string SalesId { get; set; }

        /// <summary>
        /// gyáriszám
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// számlaszám
        /// </summary>
        public string InvoiceId { get; set; }

        /// <summary>
        /// 30, 60, 90, 180, 365 nap
        /// </summary>
        public int DateIntervall { get; set; }

        /// <summary>
        /// sorrend
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// aktuális oldalindex
        /// </summary>
        public int CurrentPageIndex { get; set; }

        /// <summary>
        /// elemek száma az oldalon
        /// </summary>
        public int ItemsOnPage { get; set; }

    }
}
