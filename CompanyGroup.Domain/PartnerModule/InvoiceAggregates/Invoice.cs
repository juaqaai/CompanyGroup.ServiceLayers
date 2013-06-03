using System;
using System.Collections.Generic;

using System.Linq;

namespace CompanyGroup.Domain.PartnerModule
{
    public class Invoice
    {
        /// <summary>
        /// számla fejléc
        /// </summary>
        public InvoiceHeader Header { get; set; }

        /// <summary>
        /// számla sorok 
        /// </summary>
        public List<InvoiceLine> Lines { get; set; }

        public Invoice(InvoiceHeader header, IEnumerable<CompanyGroup.Domain.PartnerModule.InvoiceLine> lines)
        {
            this.Header = header;

            this.Lines = (lines == null) ? new List<InvoiceLine>() : lines.ToList();
        }

        public Invoice(InvoiceHeader header) : this(header, new List<InvoiceLine>()) { }

        public Invoice() : this(new InvoiceHeader(), new List<InvoiceLine>()) { }

        /// <summary>
        /// létrehoz egy számla objektumot tételeivel együtt  
        /// </summary>
        /// <param name="lineInfos"></param>
        /// <returns></returns>
        public static Invoice Create(List<InvoiceDetailedLineInfo> lineInfos)
        {
            Invoice invoice = null;

            lineInfos.ForEach(x =>
            {
                if (invoice == null)
                {
                    invoice = new Invoice(new InvoiceHeader(x.Id, x.InvoiceDate, x.SourceCompany, x.DueDate, x.InvoiceAmount, x.InvoiceCredit, x.CurrencyCode, x.InvoiceId, x.LineAmount, x.TaxAmount, x.LineAmountMst, x.TaxAmountMst, x.OverDue));
                }

                InvoiceLine line = new InvoiceLine(x.Id, x.InvoiceId, x.ItemDate, x.LineNum, x.ItemId, x.ItemName, x.Quantity, x.SalesPrice, x.LineAmount, x.QuantityPhysical, x.Remain, x.DeliveryType, x.TaxAmount, x.LineAmountMst, x.TaxAmountMst, x.CurrencyCode, x.Description, x.RecId, x.PictureExists, x.InStock,  x.AvailableInWebShop, x.DataAreaId);

                invoice.Lines.Add(line);
            });

            return invoice;
        }
    }
}
