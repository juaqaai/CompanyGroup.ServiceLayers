using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{

    [System.Runtime.Serialization.DataContract(Name = "RemoveDeliveryAddress", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class RemoveDeliveryAddress
    {
        [System.Runtime.Serialization.DataMember(Name = "RegistrationId", Order = 1)]
        public string RegistrationId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "LanguageId", Order = 2)]
        public string LanguageId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "DeliveryAddressId", Order = 3)]
        public string DeliveryAddressId { get; set; }
    }
}
