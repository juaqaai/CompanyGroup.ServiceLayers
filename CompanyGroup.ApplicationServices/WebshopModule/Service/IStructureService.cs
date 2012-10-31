using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.WebshopModule
{

    /// <summary>
    /// 
    /// </summary>
    //[ServiceContract(Namespace = "http://CompanyGroup.ApplicationServices.WebshopModule/", Name = "StructureService")]
    public interface IStructureService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[OperationContract(Action = "GetAll")]
        //[WebInvoke(Method = "POST",
        //    //UriTemplate = "/GetAll",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.Structures GetAll(CompanyGroup.Dto.ServiceRequest.GetAllStructure request);

    }
}
