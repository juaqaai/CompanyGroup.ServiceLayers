using System;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// levelezési cím 
    /// </summary>
    public class MailAddress : Address
    {
        public MailAddress() : this("", "", "", "") { }

        public MailAddress(string city, string country, string zipCode, string street) : base(city, country, zipCode, street)
        {
        }

        public override string ToString()
        {
            return "City: " + City + "; Country: " + Country + "; Street: " + Street + "; ZipCode: " + ZipCode;
        }
    }
}
