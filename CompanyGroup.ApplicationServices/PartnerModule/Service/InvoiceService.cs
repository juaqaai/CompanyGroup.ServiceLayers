using System;
using System.Collections.Generic;
using System.Linq;

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
        public CompanyGroup.Dto.PartnerModule.InvoiceInfo GetById(string invoiceId)
        {
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(invoiceId), "The invoiceId cannot be null!");

            CompanyGroup.Domain.PartnerModule.InvoiceInfo invoiceInfo = invoiceRepository.GetById(invoiceId);

            CompanyGroup.Dto.PartnerModule.InvoiceInfo result = new InvoiceInfoToInvoiceInfo().Map(invoiceInfo);

            return result;
        }

        /// <summary>
        /// vevőhöz tartozó számla lista kiolvasása
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> GetList(CompanyGroup.Dto.ServiceRequest.GetInvoiceInfo request)
        {
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(request.VisitorId), "The VisitorId cannot be null!");

            //látogató kiolvasása
            CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

            List<CompanyGroup.Domain.PartnerModule.InvoiceInfo> invoiceInfoList = invoiceRepository.GetList(visitor.CompanyId, visitor.DataAreaId);

            List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> result = invoiceInfoList.ConvertAll( x => new InvoiceInfoToInvoiceInfo().Map(x) );

            return result;
        }

        /// <summary>
        /// összes számla kiolvasása (mindkét vállalatból)
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> GetAll()
        {
            List<CompanyGroup.Domain.PartnerModule.InvoiceInfo> invoiceInfoList = invoiceRepository.GetAll();

            List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> result = invoiceInfoList.ConvertAll(x => new InvoiceInfoToInvoiceInfo().Map(x));

            return result;
        }

        /// <summary>
        /// indexek létrehozása
        /// </summary>
        public void CreateIndexes()
        {
            invoiceRepository.CreateIndexes();
        }
    }
}
