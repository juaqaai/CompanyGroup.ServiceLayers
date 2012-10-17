using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Products", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class Products
    {
        [System.Runtime.Serialization.DataMember(Name = "Items", Order = 1)]
        public List<Product> Items { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Pager", Order = 2)]
        public Pager Pager { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ListCount", Order = 3)]
        public long ListCount { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Currency", Order = 4)]
        public string Currency { get; set; }
    }
}
