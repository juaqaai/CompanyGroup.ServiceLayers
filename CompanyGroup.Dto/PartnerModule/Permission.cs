using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    [System.Runtime.Serialization.DataContract(Name = "Permission", Namespace = "CompanyGroup.Dto.PartnerModule")]
    public class Permission
    {
        [System.Runtime.Serialization.DataMember(Name = "IsWebAdministrator", Order = 1)]
        public bool IsWebAdministrator { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "InvoiceInfoEnabled", Order = 2)]
        public bool InvoiceInfoEnabled { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "PriceListDownloadEnabled", Order = 3)]
        public bool PriceListDownloadEnabled { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "CanOrder", Order = 4)]
        public bool CanOrder { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "RecieveGoods", Order = 5)]
        public bool RecieveGoods { get; set; }
    }
}
