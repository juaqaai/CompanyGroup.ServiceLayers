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
                           Pager = new CompanyGroup.ApplicationServices.PartnerModule.PagerToPager().Map(from.Pager, itemsOnPage)
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
            string invoiceMonth = (from.InvoiceDate.Month < 10) ? String.Format("0{0}", from.InvoiceDate.Month) : String.Format("{0}", from.InvoiceDate.Month);

            string invoiceDay = (from.InvoiceDate.Day < 10) ? String.Format("0{0}", from.InvoiceDate.Day) : String.Format("{0}", from.InvoiceDate.Day);

            string dueMonth = (from.DueDate.Month < 10) ? String.Format("0{0}", from.DueDate.Month) : String.Format("{0}", from.DueDate.Month);

            string dueDay = (from.DueDate.Day < 10) ? String.Format("0{0}", from.DueDate.Day) : String.Format("{0}", from.DueDate.Day);

            return new CompanyGroup.Dto.PartnerModule.InvoiceHeader(from.Id,
                                                                    String.Format("{0}.{1}.{2}", from.InvoiceDate.Year, invoiceMonth, invoiceDay), 
                                                                    from.SourceCompany,
                                                                    String.Format("{0}.{1}.{2}", from.DueDate.Year, dueMonth, dueDay),
                                                                    String.Format(((Math.Round(from.InvoiceAmount) == from.InvoiceAmount) ? "{0:0}" : "{0:0.00}"), from.InvoiceAmount),    //"{0:0,0.00}"
                                                                    String.Format(((Math.Round(from.InvoiceCredit) == from.InvoiceCredit) ? "{0:0}" : "{0:0.00}"), from.InvoiceCredit), 
                                                                    from.CurrencyCode, 
                                                                    from.InvoiceId,
                                                                    String.Format(((Math.Round(from.LineAmount) == from.LineAmount) ? "{0:0}" : "{0:0.00}"), from.LineAmount),
                                                                    String.Format(((Math.Round(from.TaxAmount) == from.TaxAmount) ? "{0:0}" : "{0:0.00}"), from.TaxAmount),
                                                                    String.Format(((Math.Round(from.LineAmountMst) == from.LineAmountMst) ? "{0:0}" : "{0:0.00}"), from.LineAmountMst),
                                                                    String.Format(((Math.Round(from.TaxAmountMst) == from.TaxAmountMst) ? "{0:0}" : "{0:0.00}"), from.TaxAmountMst));
        }

        private CompanyGroup.Dto.PartnerModule.InvoiceLine MapInvoiceLineToInvoiceLine(CompanyGroup.Domain.PartnerModule.InvoiceLine from)
        {
            string month = (from.ItemDate.Month < 10) ? String.Format("0{0}", from.ItemDate.Month) : String.Format("{0}", from.ItemDate.Month);

            string day = (from.ItemDate.Day < 10) ? String.Format("0{0}", from.ItemDate.Day) : String.Format("{0}", from.ItemDate.Day);

            return new CompanyGroup.Dto.PartnerModule.InvoiceLine(from.Id, 
                                                                  from.InvoiceId,
                                                                  from.CurrencyCode, 
                                                                  from.DeliveryType, 
                                                                  from.Description,
                                                                  String.Format("{0}.{1}.{2}", from.ItemDate.Year, month, day), 
                                                                  from.ItemId,
                                                                  String.Format(((Math.Round(from.LineAmount) == from.LineAmount) ? "{0:0}" : "{0:0.00}"), from.LineAmount),
                                                                  String.Format(((Math.Round(from.LineAmountMst) == from.LineAmountMst) ? "{0:0}" : "{0:0.00}"), from.LineAmountMst), 
                                                                  from.ItemName, 
                                                                  from.PictureExists, 
                                                                  from.Quantity,
                                                                  String.Format(((Math.Round(from.SalesPrice) == from.SalesPrice) ? "{0:0}" : "{0:0.00}"), from.SalesPrice),
                                                                  String.Format(((Math.Round(from.TaxAmount) == from.TaxAmount) ? "{0:0}" : "{0:0.00}"), from.TaxAmount),
                                                                  String.Format(((Math.Round(from.TaxAmountMst) == from.TaxAmountMst) ? "{0:0}" : "{0:0.00}"), from.TaxAmountMst), 
                                                                  from.RecId, 
                                                                  from.InStock, 
                                                                  from.AvailableInWebShop, 
                                                                  from.DataAreaId);
                                                                    
                                                                    
        }
    }
}
