using System;
using System.Collections.Generic;
using CompanyGroup.Dto.WebshopModule;

namespace CompanyGroup.Dto.ServiceResponse
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "RemoveCart", Namespace = "CompanyGroup.Dto.ServiceResponse")]
    public class RemoveCart
    {
        [System.Runtime.Serialization.DataMember(Name = "StoredItems", Order = 1)]
        public List<StoredShoppingCart> StoredItems { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "OpenedItems", Order = 2)]
        public List<OpenedShoppingCart> OpenedItems { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ActiveCart", Order = 3)]
        public CompanyGroup.Dto.WebshopModule.ShoppingCart ActiveCart { get; set; }
    }
}
