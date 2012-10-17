using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Newsletter.ApplicationService
{
    [ServiceContract(Namespace = "http://Newsletter.ApplicationService/", Name = "Service")]
    public interface IService
    {
        /// <summary>
        /// hírlevél kiküldés azonosító alapján
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract(Action = "Send")]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "/Send/{id}",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        [WebGet(UriTemplate = "/Send/{id}", BodyStyle = WebMessageBodyStyle.Bare)]  
        int Send(string id);
    }
}
