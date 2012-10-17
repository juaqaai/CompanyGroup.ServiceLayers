using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "SecondHandList", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class SecondHandList
    {
        [System.Runtime.Serialization.DataMember(Name = "MinimumPrice", Order = 2)]
        public string MinimumPrice { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Items", Order = 1)]
        public List<SecondHand> Items { get; set; }
    }

    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "SecondHand", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class SecondHand
    {
        [System.Runtime.Serialization.DataMember(Name = "ConfigId", Order = 1)]
        public string ConfigId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "InventLocationId", Order = 2)]
        public string InventLocationId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Quantity", Order = 3)]
        public int Quantity { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Price", Order = 4)]
        public int Price { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "StatusDescription", Order = 5)]
        public string StatusDescription { get; set; }
    }
}
