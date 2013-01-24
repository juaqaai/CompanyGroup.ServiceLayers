using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// kiszállítás paraméterek
    /// </summary>
    public class Shipping
    {
        public Shipping() : this(DateTime.MinValue, String.Empty, String.Empty, String.Empty, String.Empty, 0, false)
        { 
        }

        public Shipping(DateTime dateTime, string zipCode, string city, string country, string street, long addrRecId, bool invoiceAttached)
        {
            this.DateRequested = dateTime;
            this.ZipCode = zipCode;
            this.City = city;
            this.Country = country;
            this.Street = street;
            this.AddrRecId = addrRecId;
            this.InvoiceAttached = invoiceAttached;
        }

        public DateTime DateRequested { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Street { get; set; }

        public long AddrRecId { get; set; }

        public bool InvoiceAttached { get; set; }
    }
}
