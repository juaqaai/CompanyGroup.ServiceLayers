using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// számlainformációk lekérdezés paramétereit összefogó típus
    /// </summary>
    public class GetInvoiceInfoRequest
    {
        public GetInvoiceInfoRequest() : this(String.Empty, String.Empty, true, true, String.Empty, String.Empty,
                                              String.Empty, String.Empty, String.Empty, 0,
                                              1, 30) { }

        public GetInvoiceInfoRequest(string visitorId, string languageId, bool debit, bool overdue, 
                                     string itemId, string itemName, string salesId, string serialNumber, string invoiceId, int dateIntervall,
                                     int currentPageIndex, int itemsOnPage)
        {
            this.VisitorId = visitorId;

            this.LanguageId = languageId;

            this.Debit = debit;

            this.Overdue = overdue;

            this.ItemId = itemId;

            this.ItemName = itemId;

            this.SalesId = salesId;
            
            this.SerialNumber = serialNumber;
            
            this.InvoiceId = invoiceId;
            
            this.DateIntervall = dateIntervall;

            this.CurrentPageIndex = currentPageIndex;
            
            this.ItemsOnPage = itemsOnPage;
        }

        public string VisitorId { get; set; }

        public string LanguageId { get; set; }

        public bool Debit { get; set; }

        public bool Overdue { get; set; }

        public string ItemId { get; set; }

        public string ItemName { get; set; }

        public string SalesId { get; set; }
        
        public string SerialNumber { get; set; }
        
        public string InvoiceId { get; set; } 
        
        public int DateIntervall { get; set; }

        public int CurrentPageIndex { get; set; } 
        
        public int ItemsOnPage { get; set; }
    }
}
