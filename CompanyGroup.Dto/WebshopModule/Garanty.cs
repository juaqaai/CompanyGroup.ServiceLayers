using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Garanty", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class Garanty
    {
        [System.Runtime.Serialization.DataMember(Name = "Time", Order = 1)]
        public string Time { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Mode", Order = 2)]
        public string Mode { get; set; }
    }
}
