using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// structure
    /// WCF service layer DTO
    /// </summary>
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Structure", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class Structures
    {
        [System.Runtime.Serialization.DataMember(Name = "Manufacturers", Order = 1)]
        public List<Manufacturer> Manufacturers { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "FirstLevelCategories", Order = 2)]
        public List<Category> FirstLevelCategories { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "SecondLevelCategories", Order = 3)]
        public List<Category> SecondLevelCategories { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ThirdLevelCategories", Order = 4)]
        public List<Category> ThirdLevelCategories { get; set; }
    }
}
