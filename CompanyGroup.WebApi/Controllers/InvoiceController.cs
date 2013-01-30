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
        /// konstruktor service interfésszel
        /// </summary>
        /// <param name="service"></param>
        public InvoiceController(CompanyGroup.ApplicationServices.PartnerModule.IInvoiceService partnerService)
        {
            if (partnerService == null)
            {
                throw new ArgumentNullException("PartnerService");
            }

            this.partnerService = partnerService;
        }

        /// <summary>
        /// vevőhöz tartozó számlák listája
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetList")]
        [HttpPost]
        public List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> GetList(CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest request)
        {
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

    }



}
