using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// cím
    /// </summary>
    public class Address : CompanyGroup.Domain.Core.ValueObject<Address>
    {
        /// <summary>
        /// Get or set the city of this address 
        /// </summary>
        public string City { get; private set; }

        /// <summary>
        /// Get or set the zip code
        /// </summary>
        public string ZipCode { get; private set; }

        /// <summary>
        /// Get or set address line 1
        /// </summary>
        public string AddressLine { get; private set; }

        #region Constructor

        /// <summary>
        /// Create a new instance of address specifying its values
        /// </summary>
        /// <param name="city"></param>
        /// <param name="zipCode"></param>
        /// <param name="addressLine"></param>
        public Address(string city, string zipCode, string addressLine)
        {
            this.City = city;
            this.ZipCode = zipCode;
            this.AddressLine = addressLine;
        }

        /// <summary>
        /// Create a new instance
        /// <remarks>
        /// This is a "requirement" for the persistence infrastructure 
        /// and proxy creation :-(
        /// </remarks>
        /// </summary>
        public Address() { }

        #endregion
    }
}
