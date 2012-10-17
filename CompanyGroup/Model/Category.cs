using System;

namespace CompanyGroup.Model
{
    /// <summary>
    /// kategória, struktúra elem
    /// WCF szerviz réteg DTO
    /// </summary>
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Category", Namespace = "CompanyGroup.Model")]
    public class Category
    {

        [System.Runtime.Serialization.DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Name", Order = 2)]
        public string Name { get; set; }
    }
}
