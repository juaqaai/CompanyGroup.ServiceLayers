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
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetList")]
        [HttpPost]
        public List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> GetList(CompanyGroup.Dto.ServiceRequest.GetInvoiceInfoRequest request)
        {
            //DateTime from; DateTime to;

            //if (!DateTime.TryParse(request.FromDate, out from))
            //{
            //    from = DateTime.MinValue;
            //}

            //if (!DateTime.TryParse(request.ToDate, out to))
            //{
            //    to = DateTime.MaxValue;
            //}

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
