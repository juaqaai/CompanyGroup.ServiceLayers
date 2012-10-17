using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    [ServiceContract(Namespace = "http://CompanyGroup.ApplicationServices.PartnerModule/", Name = "ContactPersonService")]
    public interface IContactPersonService
    {
        [OperationContract(Action = "ChangePassword")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/ChangePassword",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        //[WebGet(UriTemplate = "/Client/{id}", BodyStyle = WebMessageBodyStyle.Bare)]  string prefix, string dataAreaId
        CompanyGroup.Dto.PartnerModule.ChangePassword ChangePassword(CompanyGroup.Dto.ServiceRequest.ChangePassword request);

        [OperationContract(Action = "UndoChangePassword")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/UndoChangePassword",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.PartnerModule.UndoChangePassword UndoChangePassword(CompanyGroup.Dto.ServiceRequest.UndoChangePassword request);
    }
}
