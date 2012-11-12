using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// levelezési cím
    /// </summary>
    public class MailAddress : CompanyGroup.Dto.RegistrationModule.MailAddress
    {
        public MailAddress(CompanyGroup.Dto.RegistrationModule.MailAddress mailAddress)
        {
            this.City = mailAddress.City;
            this.CountryRegionId = mailAddress.CountryRegionId;
            this.Street = mailAddress.Street;
            this.ZipCode = mailAddress.ZipCode;
        }

        public MailAddress() { }
    }

}
