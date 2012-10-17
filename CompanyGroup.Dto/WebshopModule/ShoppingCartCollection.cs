using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "ShoppingCartCollection", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class ShoppingCartCollection
    {
        [System.Runtime.Serialization.DataMember(Name = "Carts", Order = 1)]
        public List<ShoppingCart> Carts { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ActiveCartId", Order = 2)]
        public string ActiveCartId { get; set; }
    }
}
