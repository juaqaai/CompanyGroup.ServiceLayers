using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "StoredOpenedShoppingCartCollection", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class StoredOpenedShoppingCartCollection
    {
        public StoredOpenedShoppingCartCollection()
        {
            this.StoredItems = new List<StoredShoppingCart>();

            this.OpenedItems = new List<OpenedShoppingCart>();
        }

        [System.Runtime.Serialization.DataMember(Name = "StoredItems", Order = 1)]
        public List<StoredShoppingCart> StoredItems { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "OpenedItems", Order = 2)]
        public List<OpenedShoppingCart> OpenedItems { get; set; }
    }

    
}
