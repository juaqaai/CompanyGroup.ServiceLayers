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
    public class SalesOrderController : ApiController
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
        [AttributeRouting.Web.Http.POST("GetOrderInfo")]
        public List<CompanyGroup.Dto.PartnerModule.OrderInfo> GetOrderInfo(CompanyGroup.Dto.ServiceRequest.GetOrderInfo request)
        {
            return service.GetOrderInfo(request);
        }
    }
}
