using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.GlobalServices
{
    [ServiceContract(Namespace = "http://CompanyGroup.GlobalServices/", Name = "ProductService")]
    public interface IProductService
    {
        /// <summary>
        /// összes, a feltételeknek megfelelő termékelem leválogatása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract(Action = "GetAll")]
        [WebInvoke(Method = "POST",
            //UriTemplate = "/GetAll",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.GlobalServices.Dto.GetAllResponse GetAll(CompanyGroup.GlobalServices.Dto.GetAllRequest request);

        /// <summary>
        /// productId és dataAreaId összetett kulccsal rendelkező termékelem lekérdezése
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        [OperationContract(Action = "GetItemByProductId")]
        [WebInvoke(Method = "POST",
            //UriTemplate = "/GetItemByProductId",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.GlobalServices.Dto.GetItemByProductIdResponse GetItemByProductId(CompanyGroup.GlobalServices.Dto.GetItemByProductIdRequest request);
    }
}
