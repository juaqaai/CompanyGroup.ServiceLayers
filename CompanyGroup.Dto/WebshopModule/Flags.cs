using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Flags", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class Flags
    {
        [System.Runtime.Serialization.DataMember(Name = "New", Order = 3)]
        public bool New { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "IsInNewsletter", Order = 4)]
        public bool IsInNewsletter { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "InStock", Order = 5)]
        public bool InStock { get; set; }
    }
}
