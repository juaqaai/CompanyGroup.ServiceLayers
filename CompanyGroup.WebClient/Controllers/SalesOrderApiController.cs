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

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            CompanyGroup.Dto.ServiceRequest.GetOrderInfo req = new CompanyGroup.Dto.ServiceRequest.GetOrderInfo()
            {
                LanguageId = visitorData.Language,
                VisitorId = visitorData.ObjectId,
            };

            List<CompanyGroup.Dto.PartnerModule.OrderInfo> response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetOrderInfo, List<CompanyGroup.Dto.PartnerModule.OrderInfo>>("SalesOrder", "GetOrderInfo", req);

            List<CompanyGroup.WebClient.Models.OrderInfo> orderInfoList = new List<CompanyGroup.WebClient.Models.OrderInfo>();

            orderInfoList.AddRange(response.ConvertAll(x => new CompanyGroup.WebClient.Models.OrderInfo(x)));

            CompanyGroup.WebClient.Models.OrderInfoList model = new CompanyGroup.WebClient.Models.OrderInfoList(orderInfoList);

            return model;
        }
    }
}
