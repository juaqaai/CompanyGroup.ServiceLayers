using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// számla sor info
    /// </summary>
    public class InvoiceLineInfo : CompanyGroup.Dto.PartnerModule.InvoiceLineInfo
    {
        public InvoiceLineInfo(CompanyGroup.Dto.PartnerModule.InvoiceLineInfo invoiceLineInfo)
        { 
            this.CurrencyCode = invoiceLineInfo.CurrencyCode;
            this.DeliveryType = invoiceLineInfo.DeliveryType;
            this.ItemDate = invoiceLineInfo.ItemDate;
            this.ItemId = invoiceLineInfo.ItemId;
            this.LineAmount = invoiceLineInfo.LineAmount;
            this.Name = invoiceLineInfo.Name;
            this.TaxAmount = invoiceLineInfo.TaxAmount;
            this.Quantity = invoiceLineInfo.Quantity;
            this.SalesPrice = invoiceLineInfo.SalesPrice;
        }
    }
}
