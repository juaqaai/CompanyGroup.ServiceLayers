using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// részletes számla sor DTO
    /// </summary>
    public class InvoiceInfoDetailed
    {
        /// <summary>
        /// számla sorok
        /// </summary>
        public List<InvoiceLine> Lines { get; set; }

        /// <summary>
        /// konstruktor számla sor listával
        /// </summary>
        /// <param name="lines"></param>
        public InvoiceInfoDetailed(List<InvoiceLine> lines)
        {
            this.Lines = lines;
        }

        /// <summary>
        /// üres konstruktor, létrehozza a számla sor listát
        /// </summary>
        public InvoiceInfoDetailed()
        {
            this.Lines = new List<InvoiceLine>();
        }
    }
}
