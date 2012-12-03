using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    /// <summary>
    /// számlák szerviz partner modul
    /// </summary>
    public class InvoiceService : IInvoiceService
    {
        private CompanyGroup.Domain.PartnerModule.IInvoiceRepository invoiceRepository;

        public InvoiceService(CompanyGroup.Domain.PartnerModule.IInvoiceRepository invoiceRepository)
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
        public List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> GetList(string customerId, string dataAreaId)
        {
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(customerId), "The customerId cannot be null!");

            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(dataAreaId), "The dataareaId cannot be null!");

            List<CompanyGroup.Domain.PartnerModule.InvoiceInfo> invoiceInfoList = invoiceRepository.GetList(customerId, dataAreaId);

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
    }
}
