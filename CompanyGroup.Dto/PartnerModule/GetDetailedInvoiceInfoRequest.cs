using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// számlainformációk lekérdezés paramétereit összefogó típus
    /// </summary>
    public class GetDetailedInvoiceInfoRequest
    {
        public GetDetailedInvoiceInfoRequest() : this(String.Empty, String.Empty, 0) { }

        public GetDetailedInvoiceInfoRequest(string visitorId, string languageId, int id)
        {
            this.VisitorId = visitorId;

            this.LanguageId = languageId;

            this.Id = id;
        }

        public string VisitorId { get; set; }

        public string LanguageId { get; set; }

        public int Id { get; set; } 
        
    }
}
