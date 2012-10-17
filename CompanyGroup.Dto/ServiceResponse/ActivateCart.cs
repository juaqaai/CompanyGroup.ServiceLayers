using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceResponse
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "ActivateCart", Namespace = "CompanyGroup.Dto.ServiceResponse")]
    public class ActivateCart
    {
        [System.Runtime.Serialization.DataMember(Name = "StoredItems", Order = 1)]
        public List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> StoredItems { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "OpenedItems", Order = 2)]
        public List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> OpenedItems { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ActiveCart", Order = 3)]
        public CompanyGroup.Dto.WebshopModule.ShoppingCart ActiveCart { get; set; }
    }
}
