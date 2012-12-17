using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Stock", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class Stock
    {
        [System.Runtime.Serialization.DataMember(Name = "Inner", Order = 1)]
        public int Inner { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Outer", Order = 2)]
        public int Outer { get; set; }
    }
}
