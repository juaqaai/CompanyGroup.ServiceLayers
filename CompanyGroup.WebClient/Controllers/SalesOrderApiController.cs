﻿using System;
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

            List<CompanyGroup.Dto.PartnerModule.OrderInfo> response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.GetOrderInfoRequest, List<CompanyGroup.Dto.PartnerModule.OrderInfo>>("SalesOrder", "GetOrderInfo", req);

            List<CompanyGroup.WebClient.Models.OrderInfo> orderInfoList = new List<CompanyGroup.WebClient.Models.OrderInfo>();

            if (response != null)
            {
                orderInfoList.AddRange(response.ConvertAll(x => new CompanyGroup.WebClient.Models.OrderInfo(x)));
            }

            CompanyGroup.WebClient.Models.OrderInfoList model = new CompanyGroup.WebClient.Models.OrderInfoList(orderInfoList, visitor);

            return model;
        }
    }
}
