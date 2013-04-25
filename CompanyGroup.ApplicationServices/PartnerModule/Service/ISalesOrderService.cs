using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    public interface ISalesOrderService
    {
        /// <summary>
        /// megrendelés információk lista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[OperationContract(Action = "GetOrderInfo")]
        //[WebInvoke(Method = "POST",
        //    //UriTemplate = "/GetOrderInfo",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.PartnerModule.OrderInfoList GetOrderInfo(CompanyGroup.Dto.PartnerModule.GetOrderInfoRequest request);
    }
}
