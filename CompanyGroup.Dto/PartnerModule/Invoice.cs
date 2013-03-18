using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Dto.PartnerModule
{
    public class Invoice
    {
        /// <summary>
        /// számla fejléc lista
        /// </summary>
        public InvoiceHeader Header { get; set; }

        /// <summary>
        /// számla fejléc lista
        /// </summary>
        public List<InvoiceLine> Lines { get; set; }
    }
}
