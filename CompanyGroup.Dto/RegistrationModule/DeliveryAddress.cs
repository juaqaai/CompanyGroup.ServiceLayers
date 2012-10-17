using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.RegistrationModule
{
    [System.Runtime.Serialization.DataContract(Name = "DeliveryAddress", Namespace = "CompanyGroup.Dto.RegistrationModule")]
    public class DeliveryAddress
    {
        [System.Runtime.Serialization.DataMember(Name = "RecId", Order = 1)] 
        public long RecId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "City", Order = 2)] 
        public string City { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Street", Order = 3)] 
        public string Street { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ZipCode", Order = 4)] 
        public string ZipCode { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "CountryRegionId", Order = 5)] 
        public string CountryRegionId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Id", Order = 6)]
        public string Id { set; get; }
    }

    [System.Runtime.Serialization.DataContract(Name = "DeliveryAddresses", Namespace = "CompanyGroup.Dto.RegistrationModule")]
    public class DeliveryAddresses
    {
        [System.Runtime.Serialization.DataMember(Name = "Items", Order = 1)]
        public List<CompanyGroup.Dto.RegistrationModule.DeliveryAddress> Items { get; set; }

        public DeliveryAddresses()
        {
            this.Items = new List<CompanyGroup.Dto.RegistrationModule.DeliveryAddress>();
        }
    }
}
