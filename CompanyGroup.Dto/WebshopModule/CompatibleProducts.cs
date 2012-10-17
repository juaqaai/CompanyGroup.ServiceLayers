using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "CompatibleProducts", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class CompatibleProducts
    {
        [System.Runtime.Serialization.DataMember(Name = "Items", Order = 1)]
        public List<CompatibleProduct> Items { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ReverseItems", Order = 2)]
        public List<CompatibleProduct> ReverseItems { get; set; }

    }
}
