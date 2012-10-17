using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.RegistrationModule
{
    [System.Runtime.Serialization.DataContract(Name = "CompanyData", Namespace = "CompanyGroup.Dto.RegistrationModule")]
    public class CompanyData
    {
        /// <summary>
        /// regisztrációs szám
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "RegistrationNumber", Order = 1)]
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// hírlevél feliratkozás
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "NewsletterToMainEmail", Order = 2)]
        public bool NewsletterToMainEmail { get; set; }

        /// <summary>
        /// aláírási címpéldány
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "SignatureEntityFile", Order = 3)]
        public string SignatureEntityFile { get; set; }

        /// <summary>
        /// vevő azonosítója
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "CustomerId", Order = 4)]
        public string CustomerId { get; set; }

        /// <summary>
        /// vevő neve
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "CustomerName", Order = 5)]
        public string CustomerName { get; set; }

        /// <summary>
        /// adószám 1
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "VatNumber", Order = 6)]
        public string VatNumber { get; set; }

        /// <summary>
        /// EU adószám
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "EUVatNumber", Order = 7)]
        public string EUVatNumber { get; set; }

        /// <summary>
        /// központi e-mail
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "MainEmail", Order = 8)]
        public string MainEmail { get; set; }

        /// <summary>
        /// város
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "CountryRegionId", Order = 9)]
        public string CountryRegionId { get; set; }

    }
}
