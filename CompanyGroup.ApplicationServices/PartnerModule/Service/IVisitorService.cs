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
        List<string> GetRoles(CompanyGroup.Dto.ServiceRequest.VisitorInfo request);
    }
}
