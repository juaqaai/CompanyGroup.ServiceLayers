using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class GetInvoiceInfoRequest
    {
        public GetInvoiceInfoRequest()
        {
            this.VisitorId = "";

            this.LanguageId = "";

            this.Debit = true;

            this.Overdue = true;

            this.ItemId = "";

            this.ItemName = "";

            this.SalesId = "";

            this.SerialNumber = "";

            this.InvoiceId = "";

            this.DateIntervall = 0;

            this.Sequence = 0;

            this.CurrentPageIndex = 1;

            this.ItemsOnPage = 30;

            this.Items = new List<int>();
        }

        public string VisitorId { get; set; }

        public string LanguageId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Debit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Overdue { get; set; }

        public string ItemId { get; set; }

        public string ItemName { get; set; }

        public string SalesId { get; set; }

        public string SerialNumber { get; set; }

        public string InvoiceId { get; set; }

        public int DateIntervall { get; set; }

        public int Sequence { get; set; }

        public int CurrentPageIndex { get; set; }

        public int ItemsOnPage { get; set; }

        public List<int> Items { get; set; }

    }
}
