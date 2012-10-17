using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    public class MailAddressToMailAddress
    {
        /// <summary>
        /// Domain vevő levelezési cím adatok -> DTO. levelezési cím adatok   
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.MailAddress Map(CompanyGroup.Domain.PartnerModule.MailAddress from)
        {
            return new CompanyGroup.Dto.RegistrationModule.MailAddress() { City = from.City, CountryRegionId = from.Country, Street = from.Street, ZipCode = from.ZipCode };
        }
    }
}
