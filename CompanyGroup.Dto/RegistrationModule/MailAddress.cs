using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.RegistrationModule
{
    [System.Runtime.Serialization.DataContract(Name = "MailAddress", Namespace = "CompanyGroup.Dto.RegistrationModule")]
    public class MailAddress
    {
        [System.Runtime.Serialization.DataMember(Name = "City", Order = 1)] 
        public string City { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "CountryRegionId", Order = 2)]
        public string CountryRegionId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Street", Order = 3)]
        public string Street { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ZipCode", Order = 4)]
        public string ZipCode { get; set; }
    }
}
