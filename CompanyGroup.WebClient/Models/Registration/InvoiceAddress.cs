using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{

    public class InvoiceAddress : CompanyGroup.Dto.RegistrationModule.InvoiceAddress
    {
        public InvoiceAddress(CompanyGroup.Dto.RegistrationModule.InvoiceAddress invoiceAddress)
        {
            this.City = invoiceAddress.City;
            this.CountryRegionId = invoiceAddress.CountryRegionId;
            this.Phone = invoiceAddress.Phone;
            this.Street = invoiceAddress.Street;
            this.ZipCode = invoiceAddress.ZipCode;
        }

        public InvoiceAddress() { }
    }
}
