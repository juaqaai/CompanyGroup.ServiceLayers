using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    [System.Runtime.Serialization.DataContract(Name = "AddressZipCodes", Namespace = "CompanyGroup.Dto.PartnerModule")]
    public class AddressZipCodes
    {
        [System.Runtime.Serialization.DataMember(Name = "Items", Order = 1)]
        public List<string> Items { get; set; }
    }
}
