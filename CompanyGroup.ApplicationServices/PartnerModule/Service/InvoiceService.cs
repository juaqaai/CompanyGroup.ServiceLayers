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

                //számla lista
                List<CompanyGroup.Domain.PartnerModule.InvoiceDetailedLineInfo> invoiceDetailedLineInfo = invoiceRepository.GetList(visitor.CustomerId, request.Debit, request.Overdue,
                                                                                                                                    request.ItemId, request.ItemName, request.InvoiceId, request.SerialNumber,
                                                                                                                                    request.SalesId, request.DateIntervall, request.Sequence,
                                                                                                                                    request.CurrentPageIndex, request.ItemsOnPage);
                //group by InvoiceId
                IEnumerable<IGrouping<string, CompanyGroup.Domain.PartnerModule.InvoiceDetailedLineInfo>> groupedInvoiceDetailedLineInfo = invoiceDetailedLineInfo.GroupBy(x => x.InvoiceId).OrderByDescending(x => x.Key);

                List<CompanyGroup.Domain.PartnerModule.Invoice> invoices = new List<Domain.PartnerModule.Invoice>();

                //domain számla lista létrehozás 
                foreach( var lineInfo in groupedInvoiceDetailedLineInfo )
                {
                    CompanyGroup.Domain.PartnerModule.Invoice invoice = Domain.PartnerModule.Invoice.Create(lineInfo.ToList());

                    invoices.Add(invoice);
                };

                //elemek száma
                int count = invoiceRepository.GetListCount(visitor.CustomerId, request.Debit, request.Overdue,
                                                           request.ItemId, request.ItemName, request.SalesId, request.SerialNumber,
                                                           request.InvoiceId, request.DateIntervall);

                CompanyGroup.Domain.PartnerModule.Pager pager = new Domain.PartnerModule.Pager(request.CurrentPageIndex, count, request.ItemsOnPage);

                CompanyGroup.Domain.PartnerModule.InvoiceInfo invoiceInfo = new CompanyGroup.Domain.PartnerModule.InvoiceInfo(count, pager, 0);

                invoiceInfo.AddRange(invoices);

                CompanyGroup.Dto.PartnerModule.InvoiceInfo result = new InvoiceInfoToInvoiceInfo().Map(invoiceInfo, request.ItemsOnPage);

                return result;
            }
            catch (Exception ex) 
            { 
                throw ex; 
            }
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
            catch (Exception ex) 
            { 
                throw ex; 
            }
        }
    }
}
