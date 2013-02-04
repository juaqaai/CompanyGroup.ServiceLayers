using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// címhez tartozó irányítószám 
    /// </summary>
    public class AddressZipCode
    {
        /// <summary>
        /// irányítószám
        /// </summary>
        /// <param name="zipCode"></param>
        public AddressZipCode(string zipCode)
        {
            this.ZipCode = zipCode;
        }

        public string ZipCode { get; set; }
    }
}
