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
        public CompanyGroup.Dto.PartnerModule.InvoiceInfo Map(CompanyGroup.Domain.PartnerModule.InvoiceInfo from, int itemsOnPage)
        {
            return new CompanyGroup.Dto.PartnerModule.InvoiceInfo() 
                       {
                           Items = from.ConvertAll(x => MapInvoiceToInvoice(x)),
                           ListCount = from.ListCount,
                           Pager = new CompanyGroup.ApplicationServices.PartnerModule.PagerToPager().Map(from.Pager, itemsOnPage),
                           AllOverdueDebts = String.Format("{0:0,0.00}", from.AllOverdueDebts),
                           TotalNettoCredit = String.Format("{0:0,0.00}", from.TotalNettoCredit)
                        };
        }

        private CompanyGroup.Dto.PartnerModule.Invoice MapInvoiceToInvoice(CompanyGroup.Domain.PartnerModule.Invoice from)
        {
            return new CompanyGroup.Dto.PartnerModule.Invoice()
            {
                Header = MapInvoiceHeaderToInvoiceHeader(from.Header),

                Lines = from.Lines.ConvertAll(x => MapInvoiceLineToInvoiceLine(x))
            };
        }

        private CompanyGroup.Dto.PartnerModule.InvoiceHeader MapInvoiceHeaderToInvoiceHeader(CompanyGroup.Domain.PartnerModule.InvoiceHeader from)
        {
            return new CompanyGroup.Dto.PartnerModule.InvoiceHeader(from.Id,
                                                                    String.Format("{0}.{1}.{2}", from.InvoiceDate.Year, from.InvoiceDate.Month, from.InvoiceDate.Day), 
                                                                    from.SourceCompany, 
                                                                    String.Format("{0}.{1}.{2}", from.DueDate.Year, from.DueDate.Month, from.DueDate.Day),
                                                                    String.Format("{0:0,0.00}", from.InvoiceAmount),
                                                                    String.Format("{0:0,0.00}", from.InvoiceCredit), 
                                                                    from.CurrencyCode, 
                                                                    from.InvoiceId,
                                                                    String.Format("{0:0,0.00}", from.LineAmount),
                                                                    String.Format("{0:0,0.00}", from.TaxAmount),
                                                                    String.Format("{0:0,0.00}", from.LineAmountMst),  
                                                                    String.Format("{0:0,0.00}", from.TaxAmountMst));
        }

        private CompanyGroup.Dto.PartnerModule.InvoiceLine MapInvoiceLineToInvoiceLine(CompanyGroup.Domain.PartnerModule.InvoiceLine from)
        {
            return new CompanyGroup.Dto.PartnerModule.InvoiceLine(from.Id, 
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
                                                                  from.RecId);
                                                                    
                                                                    
        }
    }
}
