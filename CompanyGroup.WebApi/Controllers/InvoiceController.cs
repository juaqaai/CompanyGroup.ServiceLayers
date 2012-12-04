using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    public class InvoiceController : ApiController
    {
        /// <summary>
        /// privát partner szerviz interfész referencia
        /// </summary>
        private CompanyGroup.ApplicationServices.PartnerModule.IInvoiceService partnerService;

        /// <summary>
        /// privát karbantartó szerviz interfész referencia
        /// </summary>
        private CompanyGroup.ApplicationServices.MaintainModule.IInvoiceService maintainService;

        /// <summary>
        /// konstruktor service interfésszel
        /// </summary>
        /// <param name="service"></param>
        public InvoiceController(CompanyGroup.ApplicationServices.PartnerModule.IInvoiceService partnerService, CompanyGroup.ApplicationServices.MaintainModule.IInvoiceService maintainService)
        {
            if (partnerService == null)
            {
                throw new ArgumentNullException("PartnerService");
            }

            if (maintainService == null)
            {
                throw new ArgumentNullException("MaintainService");
            }

            this.partnerService = partnerService;

            this.maintainService = maintainService;
        }

        /// <summary>
        /// vevőhöz tartozó számlák listája
        /// </summary>
        /// <param name="visitorId"></param>
        /// <param name="languageId"></param>
        /// <param name="paymentType"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [ActionName("GetList")]
        [HttpGet]
        public List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> GetList(string visitorId, string languageId, int paymentType, string fromDate, string toDate)
        {
            DateTime from; DateTime to;

            if (!DateTime.TryParse(fromDate, out from))
            {
                from = DateTime.MinValue;
            }

            if (!DateTime.TryParse(toDate, out to))
            {
                to = DateTime.MaxValue;
            }

            CompanyGroup.Dto.ServiceRequest.GetInvoiceInfo request = new CompanyGroup.Dto.ServiceRequest.GetInvoiceInfo(visitorId, languageId, paymentType, from, to);

            return partnerService.GetList(request);
        }

        /// <summary>
        /// számla kiolvasása azonosító alapján
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        [ActionName("GetById")]
        [HttpGet]
        public CompanyGroup.Dto.PartnerModule.InvoiceInfo GetById(string invoiceId)
        {
            return partnerService.GetById(invoiceId);
        }

        /// <summary>
        /// összes számla kiolvasása (mindkét vállalatból)
        /// </summary>
        /// <returns></returns>
        [ActionName("GetAll")]
        [HttpGet]
        public List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> GetAll()
        {
            return partnerService.GetAll();
        }

        /// <summary>
        /// számla cache újratöltése
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        [ActionName("ReFillCache")]
        [HttpGet]
        public bool ReFillCache(string dataAreaId)
        {
            return maintainService.ReFillCache(dataAreaId);
        }
    }



}
