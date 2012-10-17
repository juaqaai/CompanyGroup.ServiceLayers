using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    public class CustomerToInvoiceAddress
    {
        /// <summary>
        /// Domain vevő adatok -> DTO. vevő adatok   
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.InvoiceAddress Map(CompanyGroup.Domain.PartnerModule.Customer from)
        {
            return new CompanyGroup.Dto.PartnerModule.InvoiceAddress() 
            { 
                City = from.InvoiceCity, 
                CountryRegionId = from.InvoiceCountry,
                Phone = from.InvoicePhone, 
                Street = from.InvoiceStreet, 
                ZipCode = from.InvoiceZipCode
            };
        }
    }
}
