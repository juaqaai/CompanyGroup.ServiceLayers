using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Completion", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class Completion
    {
        [System.Runtime.Serialization.DataMember(Name = "ProductId", Order = 1)]
        public string ProductId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ProductName", Order = 2)]
        public string ProductName { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "RecId", Order = 3)]
        public long RecId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "DataAreaId", Order = 4)]
        public string DataAreaId { get; set; }
    }

    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "CompletionList", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class CompletionList
    {
        [System.Runtime.Serialization.DataMember(Name = "Items", Order = 1)]
        public List<Completion> Items { get; set; }
    }
}
