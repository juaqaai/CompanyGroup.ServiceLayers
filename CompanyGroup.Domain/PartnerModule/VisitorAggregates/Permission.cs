using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// jogosultság beállítások
    ///     web adminisztrátor-e?
    ///     számla info engedélyezett-e?
    ///     árlista letöltés engedélyezett-e?
    ///     rendelés engedélyezett-e?
    ///     áruátvétel engedélyezett-e?
    /// </summary>
    public class Permission : CompanyGroup.Domain.Core.ValueObject<Permission>
    {
        public bool IsWebAdministrator { get; set; }

        public bool InvoiceInfoEnabled { get; set; }

        public bool PriceListDownloadEnabled { get; set; }

        public bool CanOrder { get; set; }

        public bool RecieveGoods { get; set; }

        public Permission(bool isWebAdministrator, bool invoiceInfoEnabled, bool priceListDownloadEnabled, bool canOrder, bool recieveGoods)
        {
            this.IsWebAdministrator = isWebAdministrator;

            this.InvoiceInfoEnabled = invoiceInfoEnabled;

            this.PriceListDownloadEnabled = priceListDownloadEnabled;

            this.CanOrder = canOrder;

            this.RecieveGoods = recieveGoods;
        }

        public Permission()
        {
            this.IsWebAdministrator = false;

            this.InvoiceInfoEnabled = false;

            this.PriceListDownloadEnabled = false;

            this.CanOrder = false;

            this.RecieveGoods = false;            
        }
    }
}
