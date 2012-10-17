using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Shipping", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class Shipping
    {
        public Shipping()
        { 
            this.DateRequested = DateTime.MinValue;
            this.ZipCode = String.Empty;
            this.City = String.Empty;
            this.Country = String.Empty;
            this.Street = String.Empty;
            this.AddrRecId = 0;
            this.InvoiceAttached = false;
        }

        [System.Runtime.Serialization.DataMember(Name = "DateRequested", Order = 1)]
        public DateTime DateRequested { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ZipCode", Order = 2)]
        public string ZipCode { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "City", Order = 3)]
        public string City { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Country", Order = 4)]
        public string Country { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Street", Order = 5)]
        public string Street { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "AddrRecId", Order = 6)]
        public long AddrRecId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "InvoiceAttached", Order = 7)]
        public bool InvoiceAttached { get; set; }
    }
}
