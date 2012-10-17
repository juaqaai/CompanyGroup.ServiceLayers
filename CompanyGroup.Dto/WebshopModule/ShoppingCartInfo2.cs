using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "ShoppingCartInfo2", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class ShoppingCartInfo2
    {
        public ShoppingCartInfo2()
        {
            this.StoredItems = new List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart>();

            this.OpenedItems = new List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart>();
        }

        [System.Runtime.Serialization.DataMember(Name = "StoredItems", Order = 1)]
        public List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> StoredItems { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "OpenedItems", Order = 2)]
        public List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> OpenedItems { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ActiveCart", Order = 3)]
        public CompanyGroup.Dto.WebshopModule.ShoppingCart ActiveCart { get; set; }
    }
}
