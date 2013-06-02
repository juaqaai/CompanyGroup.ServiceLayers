using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// vevőrendelés műveletek kontroller
    /// </summary>
    public class SalesOrderController : ApiBaseController
    {
        private CompanyGroup.ApplicationServices.PartnerModule.ISalesOrderService service;

        /// <summary>
        /// konstruktor vevőrendelés műveletek interfészt megvalósító példánnyal
        /// </summary>
        /// <param name="service"></param>
        public SalesOrderController(CompanyGroup.ApplicationServices.PartnerModule.ISalesOrderService service)
        {
            this.service = service;
        }

        /// <summary>
        /// megrendelés információk lista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetOrderInfo")]
        [HttpPost]
        public HttpResponseMessage GetOrderInfo(CompanyGroup.Dto.PartnerModule.GetOrderInfoRequest request)
        {
            try
            {
                CompanyGroup.Dto.PartnerModule.OrderInfoList response = service.GetOrderInfo(request);

                return Request.CreateResponse<CompanyGroup.Dto.PartnerModule.OrderInfoList>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter logWriter = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.EnterpriseLibraryContainer.Current.GetInstance<Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter>();

                logWriter.Write(String.Format("SalesOrderController GetOrderInfo {0} {1} {2}", ex.Message, ex.Source, ex.StackTrace), "Failure Category");

                return ThrowHttpError(ex);
            }
        }
    }
}
