using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Dto.PartnerModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "ProductManager", Namespace = "CompanyGroup.Dto.PartnerModule")]
    public class ProductManager
    {
        [System.Runtime.Serialization.DataMember(Name = "Name", Order = 1)]
        public string Name { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Email", Order = 2)]
        public string Email { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Extension", Order = 3)]
        public string Extension { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Mobile", Order = 4)]
        public string Mobile { get; set; }
    }
}
