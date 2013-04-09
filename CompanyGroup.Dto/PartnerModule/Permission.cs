using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    public class Permission
    {
        public bool IsWebAdministrator { get; set; }

        public bool InvoiceInfoEnabled { get; set; }

        public bool PriceListDownloadEnabled { get; set; }

        public bool CanOrder { get; set; }

        public bool RecieveGoods { get; set; }
    }
}
