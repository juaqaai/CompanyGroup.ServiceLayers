using System;
using System.Collections.Generic;

namespace CompanyGroup.GlobalServices.Dto
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "GetAllResponse", Namespace = "CompanyGroup.GlobalServices.Dto")]
    public class GetAllResponse
    {
        /// <summary>
        /// termékek lista
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Products", Order = 1)]
        public List<Product> Products { get; set; }

        /// <summary>
        /// válaszban érkező listaelemek száma
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "ListCount", Order = 2)]
        public long ListCount { get; set; }
    }
}
