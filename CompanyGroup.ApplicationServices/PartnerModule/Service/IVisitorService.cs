using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    //[ServiceContract(Namespace = "http://CompanyGroup.ApplicationServices.PartnerModule/", Name = "VisitorService")]
    public interface IVisitorService
    {
        /// <summary>
        /// bejelentkezés           
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[OperationContract(Action = "SignIn")]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "/SignIn",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.PartnerModule.Visitor SignIn(CompanyGroup.Dto.ServiceRequest.SignIn request);

        /// <summary>
        /// kijelentkezés   
        /// </summary>
        /// <param name="request"></param>
        //[OperationContract(Action = "SignOut")]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "/SignOut",
        //    RequestFormat = WebMessageFormat.Json, 
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        void SignOut(CompanyGroup.Dto.ServiceRequest.SignOut request);

        /// <summary>
        /// bejelentkezett látogatóhoz kapcsolódó mentett információk kiolvasása            
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[OperationContract(Action = "GetVisitorInfo")]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "/GetVisitorInfo",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.PartnerModule.Visitor GetVisitorInfo(CompanyGroup.Dto.ServiceRequest.VisitorInfo request);

        /// <summary>
        /// bejelentkezett látogatóhoz kapcsolódó jogosultságok listázása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[OperationContract(Action = "GetRoles")]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "/GetRoles",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        //List<string> GetRoles(CompanyGroup.Dto.ServiceRequest.VisitorInfo request);

        /// <summary>
        /// valutanem csere
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void ChangeCurrency(CompanyGroup.Dto.ServiceRequest.ChangeCurrency request);

        /// <summary>
        /// nyelvválasztás csere
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void ChangeLanguage(CompanyGroup.Dto.ServiceRequest.ChangeLanguage request);
    }
}
