using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// képekkel kapcsolatos műveletek
    /// </summary>
    public class PictureController : ApiController
    {
        private CompanyGroup.ApplicationServices.WebshopModule.IPictureService service;

        /// <summary>
        /// konstruktor képekkel kapcsolatos műveletek interfészt megvalósító példánnyal
        /// </summary>
        /// <param name="service"></param>
        public PictureController(CompanyGroup.ApplicationServices.WebshopModule.IPictureService service)
        {
            this.service = service;
        }

        /// <summary>
        /// képek listát visszaadó service
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        [AttributeRouting.Web.Http.POST("GetListByProduct")]
        public CompanyGroup.Dto.WebshopModule.Pictures GetListByProduct(CompanyGroup.Dto.ServiceRequest.PictureFilter request)
        {
            return this.service.GetListByProduct(request);
        }

        /// <summary>
        /// képtartalmat adja vissza 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="recId"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        [AttributeRouting.Web.Http.GET("GetItem/{ProductId}/{RecId}/{DataAreaId}/{Width}/{Height}")]
        public Stream GetItem(string productId, string recId, string dataAreaId, string maxWidth, string maxHeight) //CompanyGroup.Dto.ServiceRequest.PictureFilter request
        {
            return this.service.GetItem(productId, recId, dataAreaId, maxWidth, maxHeight);
        }
    }
}
