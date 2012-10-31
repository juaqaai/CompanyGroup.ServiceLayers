using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    //[ServiceContract(Namespace = "http://CompanyGroup.ApplicationServices.PartnerModule/", Name = "SalesOrderService")]
    public interface ISalesOrderService
    {
        //[OperationContract(Action = "Create")]
        //[WebInvoke(Method = "POST",
        //    //UriTemplate = "/Create",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.OrderFulFillment Create(CompanyGroup.Dto.ServiceRequest.SalesOrderCreate request);

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
        List<CompanyGroup.Dto.PartnerModule.OrderInfo> GetOrderInfo(CompanyGroup.Dto.ServiceRequest.GetOrderInfo request);
    }
}
