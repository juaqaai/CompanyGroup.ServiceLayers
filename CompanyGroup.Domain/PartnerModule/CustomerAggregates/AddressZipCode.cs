using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// címhez tartozó irányítószám 
    /// </summary>
    public class AddressZipCode
    {
        private string zipCode = String.Empty;

        public AddressZipCode(string zipCode)
        {
            this.zipCode = zipCode;
        }

        public string ZipCode { get { return zipCode; } }
    }
}
