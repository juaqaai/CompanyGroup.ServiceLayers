using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    public class CustomerToMailAddress
    {
        /// <summary>
        /// Domain vevő adatok -> DTO. levelezési cím    
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.MailAddress Map(CompanyGroup.Domain.PartnerModule.Customer from)
        {
            return new CompanyGroup.Dto.PartnerModule.MailAddress() 
            { 
                City = from.MailCity, 
                CountryRegionId = from.MailCountry,
                Street = from.MailStreet, 
                ZipCode = from.MailZipCode  
            };
        }
    }
}
