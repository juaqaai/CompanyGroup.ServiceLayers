using System;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// kategória   
    /// WCF szerviz réteg DTO
    /// </summary>
    //[Serializable]
    //[System.Runtime.Serialization.DataContract(Name = "Category", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class Category
    {
        public Category() : this(String.Empty, String.Empty, String.Empty) { }

        public Category(string id, string name, string englishName)
        {
            this.Id = Id;
            this.Name = name;
            EnglishName = englishName;
        }

        //[System.Runtime.Serialization.DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "Name", Order = 2)]
        public string Name { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "EnglishName", Order = 3)]
        public string EnglishName { get; set; }
    }
}
