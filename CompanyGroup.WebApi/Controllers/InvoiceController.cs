using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// számla web api kontroller
    /// </summary>
    public class InvoiceController : ApiBaseController
    {
        /// <summary>
        /// invoice szerviz interfész referencia
        /// </summary>
        private CompanyGroup.ApplicationServices.PartnerModule.IInvoiceService service;

        /// <summary>
        /// konstruktor invoice service interfésszel
        /// </summary>
        /// <param name="service"></param>
        public InvoiceController(CompanyGroup.ApplicationServices.PartnerModule.IInvoiceService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("PartnerService");
            }

            this.service = service;
        }

        /// <summary>
        /// vevőhöz tartozó számlák listája
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetList")]
        [HttpPost]
        public HttpResponseMessage GetList(CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest request)
        {
            try
            {
                List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> response = service.GetList(request);

                return Request.CreateResponse<List<CompanyGroup.Dto.PartnerModule.InvoiceInfo>>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// számla kiolvasása azonosító alapján
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        [ActionName("GetById")]
        [HttpGet]
        public HttpResponseMessage GetById(string invoiceId)
        {
            try
            {
                CompanyGroup.Dto.PartnerModule.InvoiceInfo response = service.GetById(invoiceId);

                return Request.CreateResponse<CompanyGroup.Dto.PartnerModule.InvoiceInfo>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// összes számla kiolvasása (mindkét vállalatból)
        /// </summary>
        /// <returns></returns>
        [ActionName("GetAll")]
        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            try
            {
                List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> response = service.GetAll();

                return Request.CreateResponse<List<CompanyGroup.Dto.PartnerModule.InvoiceInfo>>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }
    }

}
