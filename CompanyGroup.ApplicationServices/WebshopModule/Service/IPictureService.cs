using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    //[ServiceContract(Namespace = "http://CompanyGroup.ApplicationServices.WebshopModule/", Name = "PictureService")]
    public interface IPictureService
    {
        //[OperationContract(Action = "GetItem")]
        //[WebInvoke(Method = "POST",
        //    //UriTemplate = "/GetItem",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        //[WebInvoke(Method = "GET", UriTemplate = "/GetItem/{ProductId}/{RecId}/{DataAreaId}/{Width}/{Height}", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        System.IO.Stream GetItem(string productId, string recId, string dataAreaId, string width, string height); //CompanyGroup.Dto.ServiceRequest.PictureFilter request

        //[OperationContract(Action = "GetItem")]
        //[WebInvoke(Method = "POST",
        //    //UriTemplate = "/GetListByProduct",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.Pictures GetListByProduct(CompanyGroup.Dto.ServiceRequest.PictureFilter request);
    }
}
