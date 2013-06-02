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
        public HttpResponseMessage GetInvoiceInfo(CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest request)
        {
            try
            {
                CompanyGroup.Dto.PartnerModule.InvoiceInfo response = service.GetInvoiceInfo(request);

                return Request.CreateResponse<CompanyGroup.Dto.PartnerModule.InvoiceInfo>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter logWriter = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.EnterpriseLibraryContainer.Current.GetInstance<Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter>();

                logWriter.Write(String.Format("InvoiceController GetInvoiceInfo {0} {1} {2}", ex.Message, ex.Source, ex.StackTrace), "Failure Category");

                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// számla elem kiolvasása 
        /// </summary>
        /// <returns></returns>
        [ActionName("GetDetails")]
        [HttpPost]
        public HttpResponseMessage GetDetails(CompanyGroup.Dto.PartnerModule.GetDetailedInvoiceInfoRequest request)
        {
            try
            {
                CompanyGroup.Dto.PartnerModule.InvoiceInfoDetailed response = service.GetDetails(request);

                return Request.CreateResponse<CompanyGroup.Dto.PartnerModule.InvoiceInfoDetailed>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter logWriter = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.EnterpriseLibraryContainer.Current.GetInstance<Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter>();

                logWriter.Write(String.Format("InvoiceController GetDetails {0} {1} {2}", ex.Message, ex.Source, ex.StackTrace), "Failure Category");

                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// összes tartozás, lejárt tartozás, pénznem lista
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [ActionName("InvoiceSumValues")]
        [HttpGet]
        public HttpResponseMessage InvoiceSumValues(string id)
        {
            try
            {
                List<CompanyGroup.Dto.PartnerModule.InvoiceSumAmount> response = service.InvoiceSumValues(id);

                return Request.CreateResponse<List<CompanyGroup.Dto.PartnerModule.InvoiceSumAmount>>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter logWriter = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.EnterpriseLibraryContainer.Current.GetInstance<Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter>();

                logWriter.Write(String.Format("InvoiceController InvoiceSumValues {0} {1} {2}", ex.Message, ex.Source, ex.StackTrace), "Failure Category");

                return ThrowHttpError(ex);
            }        
        }
    }

}
