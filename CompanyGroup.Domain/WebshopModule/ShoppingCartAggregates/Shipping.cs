using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// kiszállítási adatok a rendeléshez
    /// </summary>
    public class Shipping
    {
        public DateTime DateRequested { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Street { get; set; }

        public long AddrRecId { get; set; }

        public bool InvoiceAttached { get; set; }
    }
}
