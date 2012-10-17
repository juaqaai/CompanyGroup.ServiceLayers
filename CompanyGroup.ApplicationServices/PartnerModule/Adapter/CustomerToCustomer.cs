using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    public class CustomerToCustomer
    {
        /// <summary>
        /// Domain vevő adatok -> DTO. vevő adatok   
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.CompanyData Map(CompanyGroup.Domain.PartnerModule.Customer from)
        {
            return new CompanyGroup.Dto.RegistrationModule.CompanyData() 
            { 
                RegistrationNumber = from.CompanyRegisterNumber, 
                CustomerName = from.CustomerName,
                MainEmail = from.Email, 
                EUVatNumber = from.EUVatNumber, 
                NewsletterToMainEmail = from.Newsletter, 
                SignatureEntityFile = from.SignatureEntityFile, 
                VatNumber = from.VatNumber, 
                CountryRegionId = from.InvoiceCountry
            };
        }
    }
}
