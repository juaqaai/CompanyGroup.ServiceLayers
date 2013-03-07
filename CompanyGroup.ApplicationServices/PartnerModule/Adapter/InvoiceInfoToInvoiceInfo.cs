using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    /// <summary>
    /// Domain számlainfo -> DTO számlainfo
    /// </summary>
    public class InvoiceInfoToInvoiceInfo
    {
        /// <summary>
        /// Domain számlainfo -> DTO számlainfo
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.InvoiceInfo Map(CompanyGroup.Domain.PartnerModule.InvoiceInfo from)
        {
            return new CompanyGroup.Dto.PartnerModule.InvoiceInfo() 
                       {
                           ContactPersonId = from.ContactPersonId,
                           CurrencyCode = from.CurrencyCode,
                           CusomerRef = from.CusomerRef,
                           DueDate = String.Format("{0}.{1}.{2}", from.DueDate.Year, from.DueDate.Month, from.DueDate.Day),
                           InvoiceAmount = String.Format("{0}", from.InvoiceAmount),
                           InvoiceCredit = String.Format("{0}", from.InvoiceCredit),
                           InvoiceDate = String.Format("{0}.{1}.{2}", from.InvoiceDate.Year, from.InvoiceDate.Month, from.InvoiceDate.Day),
                           InvoiceId = from.InvoiceId,
                           InvoicingAddress = from.InvoicingAddress,
                           InvoicingName = from.InvoicingName,
                           Payment = from.Payment,
                           Printed = from.Printed,
                           ReturnItemId = from.ReturnItemId,
                           SalesType = (int) from.SalesType,
                           SalesId = from.SalesId,  
                           Lines = from.Lines.ConvertAll(x => MapInvoiceLineInfoToInvoiceLineInfo(x)) };
        }

        private CompanyGroup.Dto.PartnerModule.InvoiceLineInfo MapInvoiceLineInfoToInvoiceLineInfo(CompanyGroup.Domain.PartnerModule.InvoiceLineInfo from)
        {
            return new CompanyGroup.Dto.PartnerModule.InvoiceLineInfo()
                       {
                           CurrencyCode = from.CurrencyCode,
                           DeliveryType = from.DeliveryType,
                           ItemDate = String.Format("{0}.{1}.{2}", from.ItemDate.Year, from.ItemDate.Month, from.ItemDate.Day),
                           ItemId = from.ItemId,
                           LineAmount = String.Format("{0}", from.LineAmount),
                           Name = from.Name,
                           Quantity = from.Quantity,
                           SalesPrice = String.Format("{0}", from.SalesPrice),
                           TaxAmount = String.Format("{0}", from.TaxAmount), 
                           Description = from.Description, 
                           RecId = from.RecId, 
                           PictureExists = from.PictureExists
                       };
        }
    }
}
