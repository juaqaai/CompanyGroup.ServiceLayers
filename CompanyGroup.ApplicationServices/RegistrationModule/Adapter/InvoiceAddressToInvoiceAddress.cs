using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.ApplicationServices.RegistrationModule
{
    public class InvoiceAddressToInvoiceAddress
    {
        /// <summary>
        /// domain invoice address -> Dto invoice address
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.InvoiceAddress MapDomainToDto(CompanyGroup.Domain.RegistrationModule.InvoiceAddress from)
        {
            return new CompanyGroup.Dto.RegistrationModule.InvoiceAddress() 
                       { 
                           City = from.City, 
                           CountryRegionId = from.Country, 
                           Phone = from.Phone, 
                           Street = from.Street, 
                           ZipCode = from.ZipCode
                       };
        }

        /// <summary>
        /// Dto. invoice address -> domain invoice address
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.RegistrationModule.InvoiceAddress MapDtoToDomain(CompanyGroup.Dto.RegistrationModule.InvoiceAddress from)
        {
            return new CompanyGroup.Domain.RegistrationModule.InvoiceAddress()
            {
                City = from.City,
                Country = from.CountryRegionId,
                Phone = from.Phone,
                Street = from.Street,
                ZipCode = from.ZipCode
            };
        }
    }
}
