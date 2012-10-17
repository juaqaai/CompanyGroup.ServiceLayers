using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.RegistrationModule
{
    public class CompanyDataToCompanyData
    {
        /// <summary>
        /// Domain vevő adatok -> DTO. vevő adatok   
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.CompanyData MapDomainToDto(CompanyGroup.Domain.RegistrationModule.CompanyData from)
        {
            return new CompanyGroup.Dto.RegistrationModule.CompanyData() 
            { 
                CountryRegionId = from.CountryRegionId,
                CustomerId = from.CustomerId, 
                RegistrationNumber = from.RegistrationNumber, 
                CustomerName = from.CustomerName,
                MainEmail = from.MainEmail, 
                EUVatNumber = from.EUVatNumber,
                NewsletterToMainEmail = from.NewsletterToMainEmail, 
                SignatureEntityFile = from.SignatureEntityFile, 
                VatNumber = from.VatNumber
            };
        }

        /// <summary>
        /// DTO. vevő adatok -> Domain vevő adatok
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.RegistrationModule.CompanyData MapDtoToDomain(CompanyGroup.Dto.RegistrationModule.CompanyData from)
        {
            return new CompanyGroup.Domain.RegistrationModule.CompanyData( from.CountryRegionId, 
                                                                           from.CustomerId, 
                                                                           from.CustomerName, 
                                                                           from.EUVatNumber, 
                                                                           from.MainEmail, 
                                                                           from.NewsletterToMainEmail, 
                                                                           from.RegistrationNumber,
                                                                           from.SignatureEntityFile,
                                                                           from.VatNumber );
        }
    }
}
