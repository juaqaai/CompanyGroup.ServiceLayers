using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    public class InvoiceLineToInvoiceLine
    {
        public CompanyGroup.Dto.PartnerModule.InvoiceLine Map(CompanyGroup.Domain.PartnerModule.InvoiceLine from)
        {
            return new CompanyGroup.Dto.PartnerModule.InvoiceLine(
                from.Id,
                from.InvoiceId, 
                from.CurrencyCode,
                from.DeliveryType,
                from.Description, 
                String.Format("{0}.{1}.{2}", from.ItemDate.Year, from.ItemDate.Month, from.ItemDate.Day), 
                from.ItemId,
                String.Format("{0:0,0.00}", from.LineAmount),
                String.Format("{0:0,0.00}", from.LineAmountMst), 
                from.ItemName, 
                from.PictureExists,
                from.Quantity,
                String.Format("{0:0,0.00}", from.SalesPrice),
                String.Format("{0:0,0.00}", from.TaxAmount),
                String.Format("{0:0,0.00}", from.TaxAmountMst),
                from.RecId, 
                from.InStock, 
                from.AvailableInWebShop);
        }
    }

}
