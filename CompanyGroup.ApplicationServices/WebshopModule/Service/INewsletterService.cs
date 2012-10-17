using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    [ServiceContract(Namespace = "http://CompanyGroup.ApplicationServices.WebshopModule/", Name = "NewsletterService")]
    public interface INewsletterService
    {
        /// <summary>
        /// hírlevél gyűjetemény lekérdezése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract(Action = "GetNewsletterCollection")]
        [WebInvoke(Method = "POST",
            //UriTemplate = "/GetNewsletterCollection",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.NewsletterCollection GetNewsletterCollection(CompanyGroup.Dto.ServiceRequest.GetNewsletterCollection request);
    }
}
