using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    public class CustomerLetterToCustomerLetter
    {
        /// <summary>
        /// Domain vevő levelezési cím adatok -> DTO. levelezési cím adatok   
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.CustomerLetter Map(CompanyGroup.Domain.PartnerModule.CustomerLetter from)
        {
            return new CompanyGroup.Dto.PartnerModule.CustomerLetter() { City = from.City, Country = from.Country, Street = from.Street, ZipCode = from.ZipCode };
        }
    }
}
