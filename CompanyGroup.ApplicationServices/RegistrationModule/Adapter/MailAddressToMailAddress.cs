using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.RegistrationModule
{
    public class MailAddressToMailAddress
    {
        /// <summary>
        /// Domain vevő levelezési cím adatok -> DTO. levelezési cím adatok   
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.MailAddress MapDomainToDto(CompanyGroup.Domain.RegistrationModule.MailAddress from)
        {
            return new CompanyGroup.Dto.RegistrationModule.MailAddress() { City = from.City, CountryRegionId = from.Country, Street = from.Street, ZipCode = from.ZipCode };
        }

        /// <summary>
        /// DTO. vevő levelezési cím adatok -> Domain levelezési cím adatok   
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.RegistrationModule.MailAddress MapDtoToDomain(CompanyGroup.Dto.RegistrationModule.MailAddress from)
        {
            return new CompanyGroup.Domain.RegistrationModule.MailAddress() { City = from.City, Country = from.CountryRegionId, Street = from.Street, ZipCode = from.ZipCode };
        }
    }
}
