using System;
using System.Collections.Generic;

namespace CompanyGroup.Model
{
    /// <summary>
    /// struktúra
    /// WCF szerviz réteg DTO
    /// </summary>
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Structure", Namespace = "CompanyGroup.Model")]
    public class Structure
    {
        [System.Runtime.Serialization.DataMember(Name = "ManufacturerItems", Order = 1)]
        public Manufacturer[] Manufacturers { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Category1List", Order = 2)]
        public Category[] Category1List { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Category2List", Order = 3)]
        public Category[] Category2List { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Category3List", Order = 4)]
        public Category[] Category3List { get; set; }
    }
}
