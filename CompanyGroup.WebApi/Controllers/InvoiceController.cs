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
        /// privát szerviz interfész referencia
        /// </summary>
        private CompanyGroup.ApplicationServices.PartnerModule.IInvoiceService service;

        /// <summary>
        /// konstruktor customer service interfésszel
        /// </summary>
        /// <param name="service"></param>
        public InvoiceController(CompanyGroup.ApplicationServices.PartnerModule.IInvoiceService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("InvoiceService");
            }

            this.service = service;        
        }

        /// <summary>
        /// vevőhöz tartozó számlák listája
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetInvoiceInfo")]
        [HttpPost]
        public List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> GetInvoiceInfo(CompanyGroup.Dto.ServiceRequest.GetInvoiceInfo request)
        {
            return service.GetList(request);
        }

    }



}
