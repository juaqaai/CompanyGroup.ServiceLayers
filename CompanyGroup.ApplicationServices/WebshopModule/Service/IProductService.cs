using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    //[ServiceContract(Namespace = "http://CompanyGroup.ApplicationServices.WebshopModule/", Name = "ProductService")]
    public interface IProductService
    {
        /// <summary>
        /// összes, a feltételeknek megfelelő termékelem leválogatása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[OperationContract(Action = "GetAll")]
        //[WebInvoke(Method = "POST",
        //    //UriTemplate = "/GetAll",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.Products GetAll(CompanyGroup.Dto.ServiceRequest.GetAllProduct request);

        /// <summary>
        /// struktúrák lekérdezése  
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CompanyGroup.Dto.WebshopModule.Structures GetStructure(CompanyGroup.Dto.ServiceRequest.GetAllStructure request);

        //[OperationContract(Action = "GetBannerList")]
        //[WebInvoke(Method = "POST",
        //    //UriTemplate = "/GetAll",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.BannerList GetBannerList(CompanyGroup.Dto.ServiceRequest.GetBannerList request);

        /// <summary>
        /// összes, a feltételeknek megfelelő árlista termékelem leválogatása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[OperationContract(Action = "GetPriceList")]
        //[WebInvoke(Method = "POST",
        //    //UriTemplate = "/GetAll",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.PriceList GetPriceList(CompanyGroup.Dto.ServiceRequest.GetPriceList request);

        /// <summary>
        /// objectId egyedi kulccsal rendelkező termékelem lekérdezése
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        //[OperationContract(Action = "GetItemByObjectId")]
        //[WebInvoke(Method = "GET",
        //    //UriTemplate = "/GetItemByObjectId/{objectId}",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.Product GetItemByObjectId(string objectId, string visitorId);

        /// <summary>
        /// productId és dataAreaId összetett kulccsal rendelkező termékelem lekérdezése
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        //[OperationContract(Action = "GetItemByProductId")]
        //[WebInvoke(Method = "POST",
        //    //UriTemplate = "/GetItemByProductId",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.Product GetItemByProductId(CompanyGroup.Dto.ServiceRequest.GetItemByProductId request);

        //[OperationContract(Action = "GetCompatibleProducts")]
        //[WebInvoke(Method = "POST",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.CompatibleProducts GetCompatibleProducts(CompanyGroup.Dto.ServiceRequest.GetItemByProductId request);

        //[OperationContract(Action = "GetCompletionList")]
        //[WebInvoke(Method = "POST",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        //[WebInvoke(Method = "GET", UriTemplate = "/GetCompletionList/{DataAreaId}/{Prefix}/{CompletionType}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.CompletionList GetCompletionList(string dataAreaId, string prefix, string completionType);
    }
}
