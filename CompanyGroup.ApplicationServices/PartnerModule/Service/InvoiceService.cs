using System;
using System.Collections.Generic;
using System.Linq;
using CompanyGroup.ApplicationServices.PartnerModule;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    /// <summary>
    /// számlák szerviz partner modul
    /// </summary>
    public class InvoiceService : ServiceBase, IInvoiceService
    {
        private CompanyGroup.Domain.PartnerModule.IInvoiceRepository invoiceRepository;

        public InvoiceService(CompanyGroup.Domain.PartnerModule.IInvoiceRepository invoiceRepository, 
                              CompanyGroup.Domain.WebshopModule.IFinanceRepository financeRepository,
                              CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository) : base(financeRepository, visitorRepository)
        {
            if (invoiceRepository == null)
            {
                throw new ArgumentNullException("InvoiceRepository");
            }

            this.invoiceRepository = invoiceRepository;
        }

        /// <summary>
        /// számla információ kiolvasása
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.InvoiceInfo GetInvoiceInfo(CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(request.VisitorId), "The VisitorId cannot be null!");

                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                CompanyGroup.Helpers.DesignByContract.Ensure(visitor.IsValidLogin, "The visitor must be logged in!");

                //számla fejléc elemek lista
                List<CompanyGroup.Domain.PartnerModule.InvoiceHeader> invoiceHeaders = invoiceRepository.GetList(visitor.CustomerId, request.Debit, request.Overdue,
                                                                                                                 request.ItemId, request.ItemName, request.InvoiceId, request.SerialNumber,
                                                                                                                 request.SalesId, request.DateIntervall, request.Sequence,
                                                                                                                 request.CurrentPageIndex, request.ItemsOnPage);

                List<CompanyGroup.Domain.PartnerModule.InvoiceLine> invoiceLines = new List<Domain.PartnerModule.InvoiceLine>();

                request.Items.ForEach(x =>
                {
                    invoiceLines.AddRange(invoiceRepository.GetDetails(x));
                });


                List<CompanyGroup.Domain.PartnerModule.Invoice> invoices = new List<CompanyGroup.Domain.PartnerModule.Invoice>();

                invoiceHeaders.ForEach(x =>
                {
                    IEnumerable<CompanyGroup.Domain.PartnerModule.InvoiceLine> lines = invoiceLines.Where(y => y.InvoiceId.Equals(x.InvoiceId));

                    CompanyGroup.Domain.PartnerModule.Invoice invoice = new Domain.PartnerModule.Invoice(x, lines);

                    invoices.Add(invoice);

                });

                //elemek száma
                int count = invoiceRepository.GetListCount(visitor.CustomerId, request.Debit, request.Overdue,
                                                           request.ItemId, request.ItemName, request.SalesId, request.SerialNumber,
                                                           request.InvoiceId, request.DateIntervall);

                CompanyGroup.Domain.PartnerModule.Pager pager = new Domain.PartnerModule.Pager(request.CurrentPageIndex, count, request.ItemsOnPage);

                CompanyGroup.Domain.PartnerModule.InvoiceInfo invoiceInfo = new CompanyGroup.Domain.PartnerModule.InvoiceInfo(count, pager, 0, false);

                invoiceInfo.AddRange(invoices);

                CompanyGroup.Dto.PartnerModule.InvoiceInfo result = new InvoiceInfoToInvoiceInfo().Map(invoiceInfo, request.ItemsOnPage);

                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// vevőhöz tartozó számla lista kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.InvoiceInfoDetailed GetDetails(CompanyGroup.Dto.PartnerModule.GetDetailedInvoiceInfoRequest request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request.Id > 0), "The id cannot be null!");

                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                List<CompanyGroup.Domain.PartnerModule.InvoiceLine> invoiceLines = invoiceRepository.GetDetails(request.Id);

                //List<CompanyGroup.Domain.PartnerModule.Invoice> invoiceInfo = new List<CompanyGroup.Domain.PartnerModule.InvoiceInfo>();

                //foreach (var lineInfo in groupedLineInfos)
                //{
                //    CompanyGroup.Domain.PartnerModule.InvoiceInfo info = CompanyGroup.Domain.PartnerModule.InvoiceInfo.Create(lineInfo.ToList());

                //    invoiceInfo.Add(info);
                //}

                List<CompanyGroup.Dto.PartnerModule.InvoiceLine> lines = invoiceLines.ConvertAll(x => new InvoiceLineToInvoiceLine().Map(x));

                CompanyGroup.Dto.PartnerModule.InvoiceInfoDetailed result = new Dto.PartnerModule.InvoiceInfoDetailed(lines);

                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// összes tartozás, lejárt tartozás, pénznem lista
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Dto.PartnerModule.InvoiceSumAmount> InvoiceSumValues(string visitorId)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(visitorId), "The visitorId cannot be null!");

                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(visitorId);

                List<CompanyGroup.Domain.PartnerModule.InvoiceSumAmount> result = invoiceRepository.InvoiceSumValues(visitor.CustomerId);

                return result.ConvertAll( x => {
                        return new CompanyGroup.Dto.PartnerModule.InvoiceSumAmount(String.Format("{0:0,0.00}", x.AmountCredit), String.Format("{0:0,0.00}", x.AmountOverdue), x.CurrencyCode);
                    });
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
