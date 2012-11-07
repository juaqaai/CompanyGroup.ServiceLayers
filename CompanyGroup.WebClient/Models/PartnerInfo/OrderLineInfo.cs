using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// megrendelés sor info
    /// </summary>
    public class OrderLineInfo : CompanyGroup.Dto.PartnerModule.OrderLineInfo
    {
        public OrderLineInfo(CompanyGroup.Dto.PartnerModule.OrderLineInfo invoiceLineInfo)
        { 
            this.CurrencyCode = invoiceLineInfo.CurrencyCode;
            this.ItemId = invoiceLineInfo.ItemId;
            this.LineAmount = invoiceLineInfo.LineAmount;
            this.Name = invoiceLineInfo.Name;
            this.InventLocationId = invoiceLineInfo.InventLocationId;
            this.Payment = invoiceLineInfo.Payment;
            this.ShippingDateRequested = invoiceLineInfo.ShippingDateRequested;
            this.StatusIssue = invoiceLineInfo.StatusIssue;
            this.Quantity = invoiceLineInfo.Quantity;
            this.SalesPrice = invoiceLineInfo.SalesPrice;
        }
    }
}
