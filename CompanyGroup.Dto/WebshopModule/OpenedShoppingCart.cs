using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "OpenedShoppingCart", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class OpenedShoppingCart
    {
        public OpenedShoppingCart()
        {
            Id = String.Empty;
            Name = String.Empty;
            Active = false;
        }

        [System.Runtime.Serialization.DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Name", Order = 2)]
        public string Name { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Active", Order = 3)]
        public bool Active { get; set; }
    }
}
