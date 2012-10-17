using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.RegistrationModule
{
    [System.Runtime.Serialization.DataContract(Name = "InvoiceAddress", Namespace = "CompanyGroup.Dto.RegistrationModule")]
    public class InvoiceAddress
    {
        /// <summary>
        /// számlázási ország
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "CountryRegionId", Order = 1)]
        public string CountryRegionId { get; set; }

        /// <summary>
        /// számlázási város
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "City", Order = 2)]
        public string City { get; set; }

        /// <summary>
        /// számlázási utca
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Street", Order = 3)]
        public string Street { get; set; }

        /// <summary>
        /// számlázási irányítószám 
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "ZipCode", Order = 4)]
        public string ZipCode { get; set; }

        /// <summary>
        /// számlázási telefonszám
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Phone", Order = 5)]
        public string Phone { get; set; }
    }
}
