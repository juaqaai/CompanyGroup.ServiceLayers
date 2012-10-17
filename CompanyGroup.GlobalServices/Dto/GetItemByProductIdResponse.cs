using System;
using System.Collections.Generic;

namespace CompanyGroup.GlobalServices.Dto
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "GetItemByProductIdResponse", Namespace = "CompanyGroup.GlobalServices.Dto")]
    public class GetItemByProductIdResponse
    {
        [System.Runtime.Serialization.DataMember(Name = "Product", Order = 1)]
        public CompanyGroup.GlobalServices.Dto.Product Product { get; set; }
    }
}
