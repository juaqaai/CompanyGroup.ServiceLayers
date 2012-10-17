using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Pictures", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class Pictures
    {
        [System.Runtime.Serialization.DataMember(Name = "Items", Order = 1)]
        public List<Picture> Items { get; set; }
    }

    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Picture", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class Picture
    {
        [System.Runtime.Serialization.DataMember(Name = "FileName", Order = 1)]
        public string FileName { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Primary", Order = 2)]
        public bool Primary { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "RecId", Order = 3)]
        public long RecId { get; set; }
    }
}
