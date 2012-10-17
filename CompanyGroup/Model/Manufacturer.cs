using System;

namespace CompanyGroup.Model
{
    /// <summary>
    /// gyártó, struktúra elem
    /// WCF szerviz réteg DTO
    /// </summary>
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Manufacturer", Namespace = "CompanyGroup.Model")]
    public class Manufacturer
    {

        [System.Runtime.Serialization.DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }


        [System.Runtime.Serialization.DataMember(Name = "Name", Order = 2)]
        public string Name { get; set; }
    }

}
