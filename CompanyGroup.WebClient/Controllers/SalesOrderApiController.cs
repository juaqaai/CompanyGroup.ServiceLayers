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
        [HttpGet]
        [ActionName("GetOrderInfo")]
        public CompanyGroup.WebClient.Models.OrderInfoList GetOrderInfo()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.WebClient.Models.Visitor visitor = (visitorData == null) ? new CompanyGroup.WebClient.Models.Visitor() : this.GetVisitor(visitorData);

            CompanyGroup.Dto.PartnerModule.GetOrderInfoRequest req = new CompanyGroup.Dto.PartnerModule.GetOrderInfoRequest(visitorData.VisitorId, visitorData.Language);

            HttpResponseMessage response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.GetOrderInfoRequest>("SalesOrder", "GetOrderInfo", req);

            List<CompanyGroup.Dto.PartnerModule.OrderInfo> orderInfos = (response.IsSuccessStatusCode) ? response.Content.ReadAsAsync<List<CompanyGroup.Dto.PartnerModule.OrderInfo>>().Result : new List<CompanyGroup.Dto.PartnerModule.OrderInfo>();

            List<CompanyGroup.WebClient.Models.OrderInfo> orderInfoList = new List<CompanyGroup.WebClient.Models.OrderInfo>();

            if (response != null)
            {
                orderInfoList.AddRange(orderInfos.ConvertAll(x => new CompanyGroup.WebClient.Models.OrderInfo(x)));
            }

            CompanyGroup.WebClient.Models.OrderInfoList viewModel = new CompanyGroup.WebClient.Models.OrderInfoList(orderInfoList, visitor);

            return viewModel;
        }
    }
}
