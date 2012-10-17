using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.RegistrationModule
{
    /// <summary>
    /// levelezési cím adatok
    /// </summary>
    public class MailAddress : CompanyGroup.Domain.PartnerModule.Address
    {
        public MailAddress() : this("", "", "", "") { }

        public MailAddress(string city, string country, string zipCode, string street) : base(city, country, zipCode, street)
        {
        }
    }
}
