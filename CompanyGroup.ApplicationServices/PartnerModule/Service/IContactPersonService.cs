using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    //[ServiceContract(Namespace = "http://CompanyGroup.ApplicationServices.PartnerModule/", Name = "ContactPersonService")]
    public interface IContactPersonService
    {
        /// <summary>
        /// jelszómódosítás
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[OperationContract(Action = "ChangePassword")]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "/ChangePassword",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        ////[WebGet(UriTemplate = "/Client/{id}", BodyStyle = WebMessageBodyStyle.Bare)]  string prefix, string dataAreaId
        CompanyGroup.Dto.PartnerModule.ChangePassword ChangePassword(CompanyGroup.Dto.ServiceRequest.ChangePassword request);

        /// <summary>
        /// jelszómódosítás visszavonás
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[OperationContract(Action = "UndoChangePassword")]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "/UndoChangePassword",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.PartnerModule.UndoChangePassword UndoChangePassword(CompanyGroup.Dto.ServiceRequest.UndoChangePassword request);

        /// <summary>
        /// kapcsolattartó lekérdezése azonosító alapján
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[OperationContract(Action = "GetContactPersonById")]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "/GetContactPersonById",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.PartnerModule.ContactPerson GetContactPersonById(CompanyGroup.Dto.ServiceRequest.GetContactPersonById request);

        //[OperationContract(Action = "ForgetPassword")]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "/ForgetPassword",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.PartnerModule.ForgetPassword ForgetPassword(CompanyGroup.Dto.ServiceRequest.ForgetPassword request);
    }
}
