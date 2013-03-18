using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// számla sor info
    /// </summary>
    public class InvoiceLineInfo : CompanyGroup.Dto.PartnerModule.InvoiceInfoDetailed
    {
        public InvoiceLineInfo(CompanyGroup.Dto.PartnerModule.InvoiceInfoDetailed invoiceLineInfo)
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
            this.Description = invoiceLineInfo.Description;
            this.PictureExists = invoiceLineInfo.PictureExists;
            this.RecId = invoiceLineInfo.RecId;
        }
    }
}
