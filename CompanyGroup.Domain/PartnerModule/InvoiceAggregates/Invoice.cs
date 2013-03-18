using System;
using System.Collections.Generic;

using System.Linq;

namespace CompanyGroup.Domain.PartnerModule
{
    public class Invoice
    {
        public InvoiceHeader Header { get; set; }

        /// <summary>
        /// számla sorok 
        /// </summary>
        public List<InvoiceLine> Lines { get; set; }

        public Invoice(InvoiceHeader header, IEnumerable<CompanyGroup.Domain.PartnerModule.InvoiceLine> lines)
        {
            this.Header = header;

            this.Lines = (lines == null) ? new List<InvoiceLine>() : lines.ToList();
        }

        public Invoice(InvoiceHeader heade) : this(heade, new List<InvoiceLine>()) { }

        public Invoice() : this(new InvoiceHeader(), new List<InvoiceLine>()) { }
    }
}
