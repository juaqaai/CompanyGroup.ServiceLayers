using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    public class InvoiceInfoDetailed
    {
        /// <summary>
        /// számla sorok
        /// </summary>
        public List<InvoiceLine> Lines { get; set; }

        public InvoiceInfoDetailed(List<InvoiceLine> lines)
        {
            this.Lines = lines;
        }

        public InvoiceInfoDetailed()
        {
            this.Lines = new List<InvoiceLine>();
        }
    }
}
