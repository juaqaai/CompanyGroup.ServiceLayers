using System;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// gyártó
    /// WCF szerviz réteg DTO
    /// </summary>
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Manufacturer", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class Manufacturer
    {
        [System.Runtime.Serialization.DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Name", Order = 2)]
        public string Name { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "EnglishName", Order = 3)]
        public string EnglishName { get; set; }
    }

}
