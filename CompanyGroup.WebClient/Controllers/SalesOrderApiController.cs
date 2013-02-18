using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebClient.Controllers
{
    public class SalesOrderApiController : ApiBaseController
    {
        /// <summary>
        /// nyitott vevőrendelés info lista
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        [ActionName("GetOrderInfo")]
        public HttpResponseMessage GetOrderInfo(CompanyGroup.WebClient.Models.GetOrderInfoRequest request)
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                CompanyGroup.WebClient.Models.Visitor visitor = (visitorData == null) ? new CompanyGroup.WebClient.Models.Visitor() : this.GetVisitor(visitorData);

                CompanyGroup.Dto.PartnerModule.GetOrderInfoRequest req = new CompanyGroup.Dto.PartnerModule.GetOrderInfoRequest(visitorData.VisitorId, visitorData.Language, request.OnOrder, request.Reserved, request.ReservedOrdered);

                HttpResponseMessage response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.GetOrderInfoRequest>("SalesOrder", "GetOrderInfo", req);

                List<CompanyGroup.Dto.PartnerModule.OrderInfo> orderInfos = (response.IsSuccessStatusCode) ? response.Content.ReadAsAsync<List<CompanyGroup.Dto.PartnerModule.OrderInfo>>().Result : new List<CompanyGroup.Dto.PartnerModule.OrderInfo>();

                CompanyGroup.WebClient.Models.OrderInfoList viewModel = new CompanyGroup.WebClient.Models.OrderInfoList(orderInfos, visitor);

                return Request.CreateResponse <CompanyGroup.WebClient.Models.OrderInfoList>(HttpStatusCode.OK, viewModel);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

    }
}
