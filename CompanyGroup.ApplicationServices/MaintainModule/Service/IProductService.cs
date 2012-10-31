using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.MaintainModule
{
    //[ServiceContract(Namespace = "http://CompanyGroup.ApplicationServices.MaintainModule/", Name = "ProductService")]
    public interface IProductService
    {
        /// <summary>
        /// terméklista cache újra töltése
        /// </summary>
        //[OperationContract(Action = "FillProductCache")]
        //[WebInvoke(Method = "GET", UriTemplate = "/FillProductCache/{DataAreaId}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool FillProductCache(string dataAreaId);

        /// <summary>
        /// terméklista cache törlése
        /// </summary>
        /// <returns></returns>
        //[OperationContract(Action = "ClearProductCache")]
        //[WebInvoke(Method = "GET", UriTemplate = "/ClearProductCache", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool ClearProductCache();

        /// <summary>
        /// terméklista cache törlése, betöltése
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        //[OperationContract(Action = "RefillProductCache")]
        //[WebInvoke(Method = "GET", UriTemplate = "/RefillProductCache/{DataAreaId}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool RefillProductCache(string dataAreaId);
    }
}
